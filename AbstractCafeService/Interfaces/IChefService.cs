using AbstractCafeService.Attributies;
using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System.Collections.Generic;

namespace AbstractCafeService.Interfaces
{
    [CustomInterface("Интерфейс для работы с работниками")]
    public interface IChefService
    {
        [CustomMethod("Метод получения списка работников")]
        List<ChefViewModel> GetList();

        [CustomMethod("Метод получения работника по id")]
        ChefViewModel GetElement(int id);

        [CustomMethod("Метод добавления работника")]
        void AddElement(ChefBindingModel model);

        [CustomMethod("Метод изменения данных по работнику")]
        void UpdElement(ChefBindingModel model);

        [CustomMethod("Метод удаления работника")]
        void DelElement(int id);
    }
}
