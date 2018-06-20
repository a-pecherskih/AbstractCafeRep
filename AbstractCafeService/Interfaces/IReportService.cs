using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    public interface IReportService
    {
        void SaveMenuPrice(ReportBindingModel model);

        List<KitchensLoadViewModel> GetKitchensLoad();

        void SaveKitchensLoad(ReportBindingModel model);

        List<CustomerChoicesModel> GetCustomerChoices(ReportBindingModel model);

        void SaveCustomerChoices(ReportBindingModel model);
    }
}
