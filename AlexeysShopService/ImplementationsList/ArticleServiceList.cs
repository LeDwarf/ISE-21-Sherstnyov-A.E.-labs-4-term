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
    public class ArticleServiceList : IArticleService
    {
        private DataListSingleton source;

        public ArticleServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ArticleViewModel> GetList()
        {
            List<ArticleViewModel> result = new List<ArticleViewModel>();
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                List<ArticlePartViewModel> productParts = new List<ArticlePartViewModel>();
                for (int j = 0; j < source.ArticleParts.Count; ++j)
                {
                    if (source.ArticleParts[j].ArticleId == source.Articles[i].Id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Parts.Count; ++k)
                        {
                            if (source.ArticleParts[j].PartId == source.Parts[k].Id)
                            {
                                componentName = source.Parts[k].PartName;
                                break;
                            }
                        }
                        productParts.Add(new ArticlePartViewModel
                        {
                            Id = source.ArticleParts[j].Id,
                            ArticleId = source.ArticleParts[j].ArticleId,
                            PartId = source.ArticleParts[j].PartId,
                            PartName = componentName,
                            Count = source.ArticleParts[j].Count
                        });
                    }
                }
                result.Add(new ArticleViewModel
                {
                    Id = source.Articles[i].Id,
                    ArticleName = source.Articles[i].ArticleName,
                    Cost = source.Articles[i].Cost,
                    ArticleParts = productParts
                });
            }
            return result;
        }

        public ArticleViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                List<ArticlePartViewModel> productParts = new List<ArticlePartViewModel>();
                for (int j = 0; j < source.ArticleParts.Count; ++j)
                {
                    if (source.ArticleParts[j].ArticleId == source.Articles[i].Id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Parts.Count; ++k)
                        {
                            if (source.ArticleParts[j].PartId == source.Parts[k].Id)
                            {
                                componentName = source.Parts[k].PartName;
                                break;
                            }
                        }
                        productParts.Add(new ArticlePartViewModel
                        {
                            Id = source.ArticleParts[j].Id,
                            ArticleId = source.ArticleParts[j].ArticleId,
                            PartId = source.ArticleParts[j].PartId,
                            PartName = componentName,
                            Count = source.ArticleParts[j].Count
                        });
                    }
                }
                if (source.Articles[i].Id == id)
                {
                    return new ArticleViewModel
                    {
                        Id = source.Articles[i].Id,
                        ArticleName = source.Articles[i].ArticleName,
                        Cost = source.Articles[i].Cost,
                        ArticleParts = productParts
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(ArticleBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                if (source.Articles[i].Id > maxId)
                {
                    maxId = source.Articles[i].Id;
                }
                if (source.Articles[i].ArticleName == model.ArticleName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Articles.Add(new Article
            {
                Id = maxId + 1,
                ArticleName = model.ArticleName,
                Cost = model.Cost
            });
            int maxPCId = 0;
            for (int i = 0; i < source.ArticleParts.Count; ++i)
            {
                if (source.ArticleParts[i].Id > maxPCId)
                {
                    maxPCId = source.ArticleParts[i].Id;
                }
            }
            for (int i = 0; i < model.ArticleParts.Count; ++i)
            {
                for (int j = 1; j < model.ArticleParts.Count; ++j)
                {
                    if (model.ArticleParts[i].PartId ==
                        model.ArticleParts[j].PartId)
                    {
                        model.ArticleParts[i].Count +=
                            model.ArticleParts[j].Count;
                        model.ArticleParts.RemoveAt(j--);
                    }
                }
            }
            for (int i = 0; i < model.ArticleParts.Count; ++i)
            {
                source.ArticleParts.Add(new ArticlePart
                {
                    Id = ++maxPCId,
                    ArticleId = maxId + 1,
                    PartId = model.ArticleParts[i].PartId,
                    Count = model.ArticleParts[i].Count
                });
            }
        }

        public void UpdElement(ArticleBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                if (source.Articles[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Articles[i].ArticleName == model.ArticleName &&
                    source.Articles[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Articles[index].ArticleName = model.ArticleName;
            source.Articles[index].Cost = model.Cost;
            int maxPCId = 0;
            for (int i = 0; i < source.ArticleParts.Count; ++i)
            {
                if (source.ArticleParts[i].Id > maxPCId)
                {
                    maxPCId = source.ArticleParts[i].Id;
                }
            }
            for (int i = 0; i < source.ArticleParts.Count; ++i)
            {
                if (source.ArticleParts[i].ArticleId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.ArticleParts.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.ArticleParts[i].Id == model.ArticleParts[j].Id)
                        {
                            source.ArticleParts[i].Count = model.ArticleParts[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        source.ArticleParts.RemoveAt(i--);
                    }
                }
            }
            for (int i = 0; i < model.ArticleParts.Count; ++i)
            {
                if (model.ArticleParts[i].Id == 0)
                {
                    for (int j = 0; j < source.ArticleParts.Count; ++j)
                    {
                        if (source.ArticleParts[j].ArticleId == model.Id &&
                            source.ArticleParts[j].PartId == model.ArticleParts[i].PartId)
                        {
                            source.ArticleParts[j].Count += model.ArticleParts[i].Count;
                            model.ArticleParts[i].Id = source.ArticleParts[j].Id;
                            break;
                        }
                    }
                    if (model.ArticleParts[i].Id == 0)
                    {
                        source.ArticleParts.Add(new ArticlePart
                        {
                            Id = ++maxPCId,
                            ArticleId = model.Id,
                            PartId = model.ArticleParts[i].PartId,
                            Count = model.ArticleParts[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.ArticleParts.Count; ++i)
            {
                if (source.ArticleParts[i].ArticleId == id)
                {
                    source.ArticleParts.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                if (source.Articles[i].Id == id)
                {
                    source.Articles.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }

}
