using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerViewModel> GetList();

        CustomerViewModel GetElement(int id);

        void AddElement(CustomerBindingModel model);

        void UpdElement(CustomerBindingModel model);

        void DelElement(int id);
    }
}
