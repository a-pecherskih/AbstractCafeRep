using AbstractCafeService.Attributies;
using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IMainService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<ChoiceViewModel> GetList();

        [CustomMethod("Метод создания заказа")]
        void CreateChoice(ChoiceBindingModel model);

        [CustomMethod("Метод передачи заказа в работу")]
        void TakeChoiceInWork(ChoiceBindingModel model);

        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishChoice(int id);

        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayChoice(int id);

        [CustomMethod("Метод пополнения блюда на кухне")]
        void PutDishOnKitchen(KitchenDishBindingModel model);
    }
}
