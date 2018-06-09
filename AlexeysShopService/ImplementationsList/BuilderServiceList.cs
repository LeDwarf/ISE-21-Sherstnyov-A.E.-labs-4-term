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
    public class BuilderServiceList : IBuilderService
    {
        private DataListSingleton source;

        public BuilderServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<BuilderViewModel> GetList()
        {
            List<BuilderViewModel> result = source.Builders
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
            Builder element = source.Builders.FirstOrDefault(rec => rec.Id == id);
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
            Builder element = source.Builders.FirstOrDefault(rec => rec.BuilderFIO == model.BuilderFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            int maxId = source.Builders.Count > 0 ? source.Builders.Max(rec => rec.Id) : 0;
            source.Builders.Add(new Builder
            {
                Id = maxId + 1,
                BuilderFIO = model.BuilderFIO
            });
        }

        public void UpdElement(BuilderBindingModel model)
        {
            Builder element = source.Builders.FirstOrDefault(rec =>
                                        rec.BuilderFIO == model.BuilderFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = source.Builders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.BuilderFIO = model.BuilderFIO;
        }

        public void DelElement(int id)
        {
            Builder element = source.Builders.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Builders.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
