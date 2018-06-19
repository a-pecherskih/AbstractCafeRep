using AbstractCafeService.Attributies;
using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    [CustomInterface("Интерфейс для работы с меню")]
    public interface IMenuService
    {
        [CustomMethod("Метод получения списка меню")]
        List<MenuViewModel> GetList();

        [CustomMethod("Метод получения меню по id")]
        MenuViewModel GetElement(int id);

        [CustomMethod("Метод добавления меню")]
        void AddElement(MenuBindingModel model);

        [CustomMethod("Метод изменения данных по меню")]
        void UpdElement(MenuBindingModel model);

        [CustomMethod("Метод удаления меню")]
        void DelElement(int id);
    }
}
