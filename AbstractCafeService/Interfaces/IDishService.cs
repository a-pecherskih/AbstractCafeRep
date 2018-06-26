using AbstractCafeService.Attributies;
using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    [CustomInterface("Интерфейс для работы с блюдами")]
    public interface IDishService
    {
        [CustomMethod("Метод получения списка блюд")]
        List<DishViewModel> GetList();

        [CustomMethod("Метод получения блюда по id")]
        DishViewModel GetElement(int id);

        [CustomMethod("Метод добавления блюда")]
        void AddElement(DishBindingModel model);

        [CustomMethod("Метод изменения данных по блюду")]
        void UpdElement(DishBindingModel model);

        [CustomMethod("Метод удаления блюда")]
        void DelElement(int id);
    }
}
