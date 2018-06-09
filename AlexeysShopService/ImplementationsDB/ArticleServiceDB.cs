using AlexeysShopModel;
using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AlexeysShopService.ImplementationsBD
{
	public class ArticleServiceBD : IArticleService
	{
		private AlexeysDbContext context;

		public ArticleServiceBD(AlexeysDbContext context)
		{
			this.context = context;
		}

		public List<ArticleViewModel> GetList()
		{
			List<ArticleViewModel> result = context.Articles
				.Select(rec => new ArticleViewModel
				{
					Id = rec.Id,
					ArticleName = rec.ArticleName,
					Cost = rec.Cost,
					ArticleParts = context.ArticleParts
							.Where(recPC => recPC.ArticleId == rec.Id)
							.Select(recPC => new ArticlePartViewModel
							{
								Id = recPC.Id,
								ArticleId = recPC.ArticleId,
								PartId = recPC.PartId,
								PartName = recPC.Part.PartName,
								Count = recPC.Count
							})
							.ToList()
				})
				.ToList();
			return result;
		}

		public ArticleViewModel GetElement(int id)
		{
			Article element = context.Articles.FirstOrDefault(rec => rec.Id == id);
			if (element != null)
			{
				return new ArticleViewModel
				{
					Id = element.Id,
					ArticleName = element.ArticleName,
					Cost = element.Cost,
					ArticleParts = context.ArticleParts
							.Where(recPC => recPC.ArticleId == element.Id)
							.Select(recPC => new ArticlePartViewModel
							{
								Id = recPC.Id,
								ArticleId = recPC.ArticleId,
								PartId = recPC.PartId,
								PartName = recPC.Part.PartName,
								Count = recPC.Count
							})
							.ToList()
				};
			}
			throw new Exception("Элемент не найден");
		}

		public void AddElement(ArticleBindingModel model)
		{
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					Article element = context.Articles.FirstOrDefault(rec => rec.ArticleName == model.ArticleName);
					if (element != null)
					{
						throw new Exception("Уже есть изделие с таким названием");
					}
					element = new Article
					{
						ArticleName = model.ArticleName,
						Cost = model.Cost
					};
					context.Articles.Add(element);
					context.SaveChanges();
					// убираем дубли по компонентам
					var groupParts = model.ArticleParts
												.GroupBy(rec => rec.PartId)
												.Select(rec => new
												{
													PartId = rec.Key,
													Count = rec.Sum(r => r.Count)
												});
					// добавляем компоненты
					foreach (var groupPart in groupParts)
					{
						context.ArticleParts.Add(new ArticlePart
						{
							ArticleId = element.Id,
							PartId = groupPart.PartId,
							Count = groupPart.Count
						});
						context.SaveChanges();
					}
					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public void UpdElement(ArticleBindingModel model)
		{
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					Article element = context.Articles.FirstOrDefault(rec =>
										rec.ArticleName == model.ArticleName && rec.Id != model.Id);
					if (element != null)
					{
						throw new Exception("Уже есть изделие с таким названием");
					}
					element = context.Articles.FirstOrDefault(rec => rec.Id == model.Id);
					if (element == null)
					{
						throw new Exception("Элемент не найден");
					}
					element.ArticleName = model.ArticleName;
					element.Cost = model.Cost;
					context.SaveChanges();

					// обновляем существуюущие компоненты
					var compIds = model.ArticleParts.Select(rec => rec.PartId).Distinct();
					var updateParts = context.ArticleParts
													.Where(rec => rec.ArticleId == model.Id &&
														compIds.Contains(rec.PartId));
					foreach (var updatePart in updateParts)
					{
						updatePart.Count = model.ArticleParts
														.FirstOrDefault(rec => rec.Id == updatePart.Id).Count;
					}
					context.SaveChanges();
					context.ArticleParts.RemoveRange(
										context.ArticleParts.Where(rec => rec.ArticleId == model.Id &&
																			!compIds.Contains(rec.PartId)));
					context.SaveChanges();
					// новые записи
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
						ArticlePart elementPC = context.ArticleParts
												.FirstOrDefault(rec => rec.ArticleId == model.Id &&
																rec.PartId == groupPart.PartId);
						if (elementPC != null)
						{
							elementPC.Count += groupPart.Count;
							context.SaveChanges();
						}
						else
						{
							context.ArticleParts.Add(new ArticlePart
							{
								ArticleId = model.Id,
								PartId = groupPart.PartId,
								Count = groupPart.Count
							});
							context.SaveChanges();
						}
					}
					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public void DelElement(int id)
		{
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					Article element = context.Articles.FirstOrDefault(rec => rec.Id == id);
					if (element != null)
					{
						// удаяем записи по компонентам при удалении изделия
						context.ArticleParts.RemoveRange(
											context.ArticleParts.Where(rec => rec.ArticleId == id));
						context.Articles.Remove(element);
						context.SaveChanges();
					}
					else
					{
						throw new Exception("Элемент не найден");
					}
					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}
	}
}
