using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    public interface IChefService
    {
        List<ChefViewModel> GetList();

        ChefViewModel GetElement(int id);

        void AddElement(ChefBindingModel model);

        void UpdElement(ChefBindingModel model);

        void DelElement(int id);
    }
}
