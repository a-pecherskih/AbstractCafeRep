using AbstractCafeService.Attributies;
using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    [CustomInterface("Интерфейс для работы с кухней")]
    public interface IKitchenService
    {
        [CustomMethod("Метод получения списка кухонь")]
        List<KitchenViewModel> GetList();

        [CustomMethod("Метод получения кухни по id")]
        KitchenViewModel GetElement(int id);

        [CustomMethod("Метод добавления кухни")]
        void AddElement(KitchenBindingModel model);

        [CustomMethod("Метод изменения данных по кухне")]
        void UpdElement(KitchenBindingModel model);

        [CustomMethod("Метод удаления кухни")]
        void DelElement(int id);
    }
}
