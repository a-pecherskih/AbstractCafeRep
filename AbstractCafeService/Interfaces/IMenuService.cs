using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    public interface IMenuService
    {
        List<MenuViewModel> GetList();

        MenuViewModel GetElement(int id);

        void AddElement(MenuBindingModel model);

        void UpdElement(MenuBindingModel model);

        void DelElement(int id);
    }
}
