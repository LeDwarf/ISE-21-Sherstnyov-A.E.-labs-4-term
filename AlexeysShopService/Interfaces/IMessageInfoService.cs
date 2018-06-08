using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System.Collections.Generic;

namespace AlexeysShopService.Interfaces
{
    public interface IMessageInfoService
    {
        List<MessageInfoViewModel> GetList();

        MessageInfoViewModel GetElement(int id);

        void AddElement(MessageInfoBindingModel model);
    }
}
