using AlexeysShopService.Attributies;
using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System.Collections.Generic;

namespace AlexeysShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с изделиями")]
    public interface IArticleService
    {
        [CustomMethod("Метод получения списка изделий")]
        List<ArticleViewModel> GetList();

        [CustomMethod("Метод получения изделия по id")]
        ArticleViewModel GetElement(int id);

        [CustomMethod("Метод добавления изделия")]
        void AddElement(ArticleBindingModel model);

        [CustomMethod("Метод изменения данных по изделию")]
        void UpdElement(ArticleBindingModel model);

        [CustomMethod("Метод удаления изделия")]
        void DelElement(int id);
    }
}