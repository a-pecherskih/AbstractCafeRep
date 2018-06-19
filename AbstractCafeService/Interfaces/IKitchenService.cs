using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    public interface IKitchenService
    {
        List<KitchenViewModel> GetList();

        KitchenViewModel GetElement(int id);

        void AddElement(KitchenBindingModel model);

        void UpdElement(KitchenBindingModel model);

        void DelElement(int id);
    }
}
