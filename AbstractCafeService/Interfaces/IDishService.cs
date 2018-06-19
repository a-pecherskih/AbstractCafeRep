using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    public interface IDishService
    {
        List<DishViewModel> GetList();

        DishViewModel GetElement(int id);

        void AddElement(DishBindingModel model);

        void UpdElement(DishBindingModel model);

        void DelElement(int id);
    }
}
