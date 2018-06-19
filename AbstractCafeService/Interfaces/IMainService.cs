using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    public interface IMainService
    {
        List<ChoiceViewModel> GetList();

        void CreateChoice(ChoiceBindingModel model);

        void TakeChoiceInWork(ChoiceBindingModel model);

        void FinishChoice(int id);

        void PayChoice(int id);

        void PutDishOnKitchen(KitchenDishBindingModel model);
    }
}
