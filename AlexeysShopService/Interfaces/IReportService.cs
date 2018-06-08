using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System.Collections.Generic;

namespace AlexeysShopService.Interfaces
{
    public interface IReportService
    {
        void SaveArticlePrice(ReportBindingModel model);

        List<StoragesLoadViewModel> GetStoragesLoad();

        void SaveStoragesLoad(ReportBindingModel model);

        List<CustomerContractsModel> GetCustomerContracts(ReportBindingModel model);

        void SaveCustomerContracts(ReportBindingModel model);
    }
}
