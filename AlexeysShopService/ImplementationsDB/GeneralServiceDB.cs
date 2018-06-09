using AlexeysShopModel;
using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using AlexeysShop;

namespace AlexeysShopService.ImplementationsBD
{
    public class GeneralServiceBD : IGeneralService
    {
        private AlexeysDbContext context;

        public GeneralServiceBD(AlexeysDbContext context)
        {
            this.context = context;
        }

        public List<ContractViewModel> GetList()
        {
            List<ContractViewModel> result = context.Contracts
                .Select(rec => new ContractViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    ArticleId = rec.ArticleId,
                    BuilderId = rec.BuilderId,
                    DateBegin = SqlFunctions.DateName("dd", rec.DateBegin) + " " +
                                SqlFunctions.DateName("mm", rec.DateBegin) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateBegin),
                    DateBuilt = rec.DateBuilt == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateBuilt.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateBuilt.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateBuilt.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Cost = rec.Cost,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    ArticleName = rec.Article.ArticleName,
                    BuilderName = rec.Builder.BuilderFIO
                })
                .ToList();
            return result;
        }

        public void CreateContract(ContractBindingModel model)
        {
            var Contract = new Contract
            {
                CustomerId = model.CustomerId,
                ArticleId = model.ArticleId,
                DateBegin = DateTime.Now,
                Count = model.Count,
                Cost = model.Cost,
                Status = ContractStatus.Принят
            };
            context.Contracts.Add(Contract);
            context.SaveChanges();

            var Customer = context.Customers.FirstOrDefault(x => x.Id == model.CustomerId);
            SendEmail(Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} создан успешно", Contract.Id,
                Contract.DateBegin.ToShortDateString()));
        }

        public void TakeContractInWork(ContractBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Contract element = context.Contracts.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var ArticleParts = context.ArticleParts
                                                .Include(rec => rec.Part)
                                                .Where(rec => rec.ArticleId == element.ArticleId);
                    // списываем
                    foreach (var ArticlePart in ArticleParts)
                    {
                        int countOnStorages = ArticlePart.Count * element.Count;
                        var StorageParts = context.StorageParts
                                                    .Where(rec => rec.PartId == ArticlePart.PartId);
                        foreach (var StoragePart in StorageParts)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (StoragePart.Count >= countOnStorages)
                            {
                                StoragePart.Count -= countOnStorages;
                                countOnStorages = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStorages -= StoragePart.Count;
                                StoragePart.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStorages > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                ArticlePart.Part.PartName + " требуется " +
                                ArticlePart.Count + ", не хватает " + countOnStorages);
                        }
                    }
                    element.BuilderId = model.BuilderId;
                    element.DateBuilt = DateTime.Now;
                    element.Status = ContractStatus.Выполняется;
                    context.SaveChanges();
                    SendEmail(element.Customer.Mail, "Оповещение по заказам",
                        string.Format("Заказ №{0} от {1} передеан в работу", element.Id, element.DateBegin.ToShortDateString()));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public void FinishContract(int id)
        {
            Contract element = context.Contracts.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ContractStatus.Готов;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} передан на оплату", element.Id,
                element.DateBegin.ToShortDateString()));
        }

        public void PayContract(int id)
        {
            Contract element = context.Contracts.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ContractStatus.Оплачен;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} оплачен успешно", element.Id, element.DateBegin.ToShortDateString()));
        }

        public void PutPartOnStorage(StoragePartBindingModel model)
        {
            StoragePart element = context.StorageParts
                                                .FirstOrDefault(rec => rec.StorageId == model.StorageId &&
                                                                    rec.PartId == model.PartId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.StorageParts.Add(new StoragePart
                {
                    StorageId = model.StorageId,
                    PartId = model.PartId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpCustomer = null;

            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpCustomer = new SmtpClient("smtp.gmail.com", 587);
                objSmtpCustomer.UseDefaultCredentials = false;
                objSmtpCustomer.EnableSsl = true;
                objSmtpCustomer.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpCustomer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpCustomer.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpCustomer = null;
            }
        }
    }
}
