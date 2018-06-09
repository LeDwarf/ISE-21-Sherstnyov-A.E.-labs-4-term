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
            List<ArticleViewModel> result = source.Articles
                .Select(rec => new ArticleViewModel
                {
                    Id = rec.Id,
                    ArticleName = rec.ArticleName,
                    Cost = rec.Cost,
                    ArticleParts = source.ArticleParts
                            .Where(recPC => recPC.ArticleId == rec.Id)
                            .Select(recPC => new ArticlePartViewModel
                            {
                                Id = recPC.Id,
                                ArticleId = recPC.ArticleId,
                                PartId = recPC.PartId,
                                PartName = source.Parts
                                    .FirstOrDefault(recC => recC.Id == recPC.PartId)?.PartName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ArticleViewModel GetElement(int id)
        {
            Article element = source.Articles.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ArticleViewModel
                {
                    Id = element.Id,
                    ArticleName = element.ArticleName,
                    Cost = element.Cost,
                    ArticleParts = source.ArticleParts
                            .Where(recPC => recPC.ArticleId == element.Id)
                            .Select(recPC => new ArticlePartViewModel
                            {
                                Id = recPC.Id,
                                ArticleId = recPC.ArticleId,
                                PartId = recPC.PartId,
                                PartName = source.Parts
                                        .FirstOrDefault(recC => recC.Id == recPC.PartId)?.PartName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ArticleBindingModel model)
        {
            Article element = source.Articles.FirstOrDefault(rec => rec.ArticleName == model.ArticleName);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            int maxId = source.Articles.Count > 0 ? source.Articles.Max(rec => rec.Id) : 0;
            source.Articles.Add(new Article
            {
                Id = maxId + 1,
                ArticleName = model.ArticleName,
                Cost = model.Cost
            });
            int maxPCId = source.ArticleParts.Count > 0 ?
                                    source.ArticleParts.Max(rec => rec.Id) : 0;
            var groupParts = model.ArticleParts
                                        .GroupBy(rec => rec.PartId)
                                        .Select(rec => new
                                        {
                                            PartId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupPart in groupParts)
            {
                source.ArticleParts.Add(new ArticlePart
                {
                    Id = ++maxPCId,
                    ArticleId = maxId + 1,
                    PartId = groupPart.PartId,
                    Count = groupPart.Count
                });
            }
        }

        public void UpdElement(ArticleBindingModel model)
        {
            Article element = source.Articles.FirstOrDefault(rec =>
                                        rec.ArticleName == model.ArticleName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            element = source.Articles.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ArticleName = model.ArticleName;
            element.Cost = model.Cost;

            int maxPCId = source.ArticleParts.Count > 0 ? source.ArticleParts.Max(rec => rec.Id) : 0;
            var compIds = model.ArticleParts.Select(rec => rec.PartId).Distinct();
            var updateParts = source.ArticleParts
                                            .Where(rec => rec.ArticleId == model.Id &&
                                           compIds.Contains(rec.PartId));
            foreach (var updatePart in updateParts)
            {
                updatePart.Count = model.ArticleParts
                                                .FirstOrDefault(rec => rec.Id == updatePart.Id).Count;
            }
            source.ArticleParts.RemoveAll(rec => rec.ArticleId == model.Id &&
                                       !compIds.Contains(rec.PartId));
            var groupParts = model.ArticleParts
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.PartId)
                                        .Select(rec => new
                                        {
                                            PartId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupPart in groupParts)
            {
                ArticlePart elementPC = source.ArticleParts
                                        .FirstOrDefault(rec => rec.ArticleId == model.Id &&
                                                        rec.PartId == groupPart.PartId);
                if (elementPC != null)
                {
                    elementPC.Count += groupPart.Count;
                }
                else
                {
                    source.ArticleParts.Add(new ArticlePart
                    {
                        Id = ++maxPCId,
                        ArticleId = model.Id,
                        PartId = groupPart.PartId,
                        Count = groupPart.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Article element = source.Articles.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.ArticleParts.RemoveAll(rec => rec.ArticleId == id);
                source.Articles.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }

}
