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
            List<ContractViewModel> result = new List<ContractViewModel>();
            for (int i = 0; i < source.Contracts.Count; ++i)
            {
                string clientFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if (source.Customers[j].Id == source.Contracts[i].CustomerId)
                    {
                        clientFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string productName = string.Empty;
                for (int j = 0; j < source.Articles.Count; ++j)
                {
                    if (source.Articles[j].Id == source.Contracts[i].ArticleId)
                    {
                        productName = source.Articles[j].ArticleName;
                        break;
                    }
                }
                string implementerFIO = string.Empty;
                if (source.Contracts[i].BuilderId.HasValue)
                {
                    for (int j = 0; j < source.Builders.Count; ++j)
                    {
                        if (source.Builders[j].Id == source.Contracts[i].BuilderId.Value)
                        {
                            implementerFIO = source.Builders[j].BuilderFIO;
                            break;
                        }
                    }
                }
                result.Add(new ContractViewModel
                {
                    Id = source.Contracts[i].Id,
                    CustomerId = source.Contracts[i].CustomerId,
                    CustomerFIO = clientFIO,
                    ArticleId = source.Contracts[i].ArticleId,
                    ArticleName = productName,
                    BuilderId = source.Contracts[i].BuilderId,
                    BuilderName = implementerFIO,
                    Count = source.Contracts[i].Count,
                    Cost = source.Contracts[i].Cost,
                    DateBegin = source.Contracts[i].DateBegin.ToLongDateString(),
                    DateBuilt = source.Contracts[i].DateBuilt?.ToLongDateString(),
                    Status = source.Contracts[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateContract(ContractBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Contracts.Count; ++i)
            {
                if (source.Contracts[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.Contracts.Count; ++i)
            {
                if (source.Contracts[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            for (int i = 0; i < source.ArticleParts.Count; ++i)
            {
                if (source.ArticleParts[i].ArticleId == source.Contracts[index].ArticleId)
                {
                    int countOnStorages = 0;
                    for (int j = 0; j < source.StorageParts.Count; ++j)
                    {
                        if (source.StorageParts[j].PartId == source.ArticleParts[i].PartId)
                        {
                            countOnStorages += source.StorageParts[j].Count;
                        }
                    }
                    if (countOnStorages < source.ArticleParts[i].Count * source.Contracts[index].Count)
                    {
                        for (int j = 0; j < source.Parts.Count; ++j)
                        {
                            if (source.Parts[j].Id == source.ArticleParts[i].PartId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Parts[j].PartName +
                                    " требуется " + source.ArticleParts[i].Count + ", в наличии " + countOnStorages);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < source.ArticleParts.Count; ++i)
            {
                if (source.ArticleParts[i].ArticleId == source.Contracts[index].ArticleId)
                {
                    int countOnStorages = source.ArticleParts[i].Count * source.Contracts[index].Count;
                    for (int j = 0; j < source.StorageParts.Count; ++j)
                    {
                        if (source.StorageParts[j].PartId == source.ArticleParts[i].PartId)
                        {
                            if (source.StorageParts[j].Count >= countOnStorages)
                            {
                                source.StorageParts[j].Count -= countOnStorages;
                                break;
                            }
                            else
                            {
                                countOnStorages -= source.StorageParts[j].Count;
                                source.StorageParts[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Contracts[index].BuilderId = model.BuilderId;
            source.Contracts[index].DateBuilt = DateTime.Now;
            source.Contracts[index].Status = ContractStatus.Выполняется;
        }

        public void FinishContract(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Contracts.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Contracts[index].Status = ContractStatus.Готов;
        }

        public void PayContract(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Contracts.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Contracts[index].Status = ContractStatus.Оплачен;
        }

        public void PutPartOnStorage(StoragePartBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.StorageParts.Count; ++i)
            {
                if (source.StorageParts[i].StorageId == model.StorageId &&
                    source.StorageParts[i].PartId == model.PartId)
                {
                    source.StorageParts[i].Count += model.Count;
                    return;
                }
                if (source.StorageParts[i].Id > maxId)
                {
                    maxId = source.StorageParts[i].Id;
                }
            }
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
