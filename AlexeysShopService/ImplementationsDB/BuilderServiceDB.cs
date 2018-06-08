using AlexeysShopModel;
using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlexeysShopService.ImplementationsBD
{
	public class BuilderServiceBD : IBuilderService
	{
		private AlexeysDbContext context;

		public BuilderServiceBD(AlexeysDbContext context)
		{
			this.context = context;
		}

		public List<BuilderViewModel> GetList()
		{
			List<BuilderViewModel> result = context.Builders
				.Select(rec => new BuilderViewModel
				{
					Id = rec.Id,
					BuilderFIO = rec.BuilderFIO
				})
				.ToList();
			return result;
		}

		public BuilderViewModel GetElement(int id)
		{
			Builder element = context.Builders.FirstOrDefault(rec => rec.Id == id);
			if (element != null)
			{
				return new BuilderViewModel
				{
					Id = element.Id,
					BuilderFIO = element.BuilderFIO
				};
			}
			throw new Exception("Элемент не найден");
		}

		public void AddElement(BuilderBindingModel model)
		{
			Builder element = context.Builders.FirstOrDefault(rec => rec.BuilderFIO == model.BuilderFIO);
			if (element != null)
			{
				throw new Exception("Уже есть сотрудник с таким ФИО");
			}
			context.Builders.Add(new Builder
			{
				BuilderFIO = model.BuilderFIO
			});
			context.SaveChanges();
		}

		public void UpdElement(BuilderBindingModel model)
		{
			Builder element = context.Builders.FirstOrDefault(rec =>
										rec.BuilderFIO == model.BuilderFIO && rec.Id != model.Id);
			if (element != null)
			{
				throw new Exception("Уже есть сотрудник с таким ФИО");
			}
			element = context.Builders.FirstOrDefault(rec => rec.Id == model.Id);
			if (element == null)
			{
				throw new Exception("Элемент не найден");
			}
			element.BuilderFIO = model.BuilderFIO;
			context.SaveChanges();
		}

		public void DelElement(int id)
		{
			Builder element = context.Builders.FirstOrDefault(rec => rec.Id == id);
			if (element != null)
			{
				context.Builders.Remove(element);
				context.SaveChanges();
			}
			else
			{
				throw new Exception("Элемент не найден");
			}
		}
	}
}
