using AlexeysShopService.Attributies;
using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System.Collections.Generic;

namespace AlexeysShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с работниками")]
    public interface IBuilderService
    {
        [CustomMethod("Метод получения списка работников")]
        List<BuilderViewModel> GetList();

        [CustomMethod("Метод получения работника по id")]
        BuilderViewModel GetElement(int id);

        [CustomMethod("Метод добавления работника")]
        void AddElement(BuilderBindingModel model);

        [CustomMethod("Метод изменения данных по работнику")]
        void UpdElement(BuilderBindingModel model);

        [CustomMethod("Метод удаления работника")]
        void DelElement(int id);
    }
}