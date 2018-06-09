using AlexeysShopService.Attributies;
using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System.Collections.Generic;

namespace AlexeysShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с компонентами")]
    public interface IPartService
    {
        [CustomMethod("Метод получения списка компонент")]
        List<PartViewModel> GetList();

        [CustomMethod("Метод получения компонента по id")]
        PartViewModel GetElement(int id);

        [CustomMethod("Метод добавления компонента")]
        void AddElement(PartBindingModel model);

        [CustomMethod("Метод изменения данных по компоненту")]
        void UpdElement(PartBindingModel model);

        [CustomMethod("Метод удаления компонента")]
        void DelElement(int id);
    }
}