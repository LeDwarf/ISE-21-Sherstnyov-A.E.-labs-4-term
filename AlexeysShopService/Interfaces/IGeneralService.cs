using AlexeysShopService.Attributies;
using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System.Collections.Generic;

namespace AlexeysShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IGeneralService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<ContractViewModel> GetList();

        [CustomMethod("Метод создания заказа")]
        void CreateContract(ContractBindingModel model);

        [CustomMethod("Метод передачи заказа в работу")]
        void TakeContractInWork(ContractBindingModel model);

        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishContract(int id);

        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayContract(int id);

        [CustomMethod("Метод пополнения компонент на складе")]
        void PutPartOnStorage(StoragePartBindingModel model);
    }
}