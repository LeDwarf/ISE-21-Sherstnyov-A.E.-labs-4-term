using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlexeysShopService.ViewModels;
using AlexeysShopService.BindingModels;

namespace AlexeysShopService.Interfaces
{
    public interface IBuilderService
    {
        List<BuilderViewModel> GetList();

        BuilderViewModel GetElement(int id);

        void AddElement(BuilderBindingModel model);

        void UpdElement(BuilderBindingModel model);

        void DelElement(int id);

    }
}
