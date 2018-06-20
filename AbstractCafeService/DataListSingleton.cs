using AbstractCafeModel;
using System.Collections.Generic;

namespace AbstractCafeService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Customer> Customers { get; set; }

        public List<Dish> Dishs { get; set; }

        public List<Chef> Chefs { get; set; }

        public List<Choice> Choices { get; set; }

        public List<Menu> Menus { get; set; }

        public List<MenuDish> MenuDishs { get; set; }

        public List<Kitchen> Kitchens { get; set; }

        public List<KitchenDish> KitchenDishs { get; set; }

        private DataListSingleton()
        {
            Customers = new List<Customer>();
            Dishs = new List<Dish>();
            Chefs = new List<Chef>();
            Choices = new List<Choice>();
            Menus = new List<Menu>();
            MenuDishs = new List<MenuDish>();
            Kitchens = new List<Kitchen>();
            KitchenDishs = new List<KitchenDish>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}
