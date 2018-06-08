using AlexeysShop;
using AlexeysShopModel;
using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopService.ImplementationsList
{
    public class GeneralServiceList : IGeneralService
    {
        private DataListSingleton source;

        public GeneralServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ContractViewModel> GetList()
        {
            List<ContractViewModel> result = source.Contracts
                .Select(rec => new ContractViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    ArticleId = rec.ArticleId,
                    BuilderId = rec.BuilderId,
                    DateBegin = rec.DateBegin.ToLongDateString(),
                    DateBuilt = rec.DateBuilt?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Cost = rec.Cost,
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerId)?.CustomerFIO,
                    ArticleName = source.Articles
                                    .FirstOrDefault(recP => recP.Id == rec.ArticleId)?.ArticleName,
                    BuilderName = source.Builders
                                    .FirstOrDefault(recI => recI.Id == rec.BuilderId)?.BuilderFIO
                })
                .ToList();
            return result;
        }

        public void CreateContract(ContractBindingModel model)
        {
            int maxId = source.Contracts.Count > 0 ? source.Contracts.Max(rec => rec.Id) : 0;
            source.Contracts.Add(new Contract
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                ArticleId = model.ArticleId,
                DateBegin = DateTime.Now,
                Count = model.Count,
                Cost = model.Cost,
                Status = ContractStatus.Принят
            });
        }

        public void TakeContractInWork(ContractBindingModel model)
        {
            Contract element = source.Contracts.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            var articleParts = source.ArticleParts.Where(rec => rec.ArticleId == element.ArticleId);
            foreach (var articlePart in articleParts)
            {
                int countOnStorages = source.StorageParts
                                            .Where(rec => rec.PartId == articlePart.PartId)
                                            .Sum(rec => rec.Count);
                if (countOnStorages < articlePart.Count * element.Count)
                {
                    var partName = source.Parts
                                    .FirstOrDefault(rec => rec.Id == articlePart.PartId);
                    throw new Exception("Не достаточно детали " + partName?.PartName +
                        " требуется " + articlePart.Count + ", в наличии " + countOnStorages);
                }
            }
            foreach (var articlePart in articleParts)
            {
                int countOnStorages = articlePart.Count * element.Count;
                var stockParts = source.StorageParts
                                            .Where(rec => rec.PartId == articlePart.PartId);
                foreach (var stockPart in stockParts)
                {
                    if (stockPart.Count >= countOnStorages)
                    {
                        stockPart.Count -= countOnStorages;
                        break;
                    }
                    else
                    {
                        countOnStorages -= stockPart.Count;
                        stockPart.Count = 0;
                    }
                }
            }
            element.BuilderId = model.BuilderId;
            element.DateBuilt = DateTime.Now;
            element.Status = ContractStatus.Выполняется;
        }

        public void FinishContract(int id)
        {
            Contract element = source.Contracts.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ContractStatus.Готов;
        }

        public void PayContract(int id)
        {
            Contract element = source.Contracts.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ContractStatus.Оплачен;
        }

        public void PutPartOnStorage(StoragePartBindingModel model)
        {
            StoragePart element = source.StorageParts
                                                .FirstOrDefault(rec => rec.StorageId == model.StorageId &&
                                                                    rec.PartId == model.PartId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StorageParts.Count > 0 ? source.StorageParts.Max(rec => rec.Id) : 0;
                source.StorageParts.Add(new StoragePart
                {
                    Id = ++maxId,
                    StorageId = model.StorageId,
                    PartId = model.PartId,
                    Count = model.Count
                });
            }
        }
    }
}
