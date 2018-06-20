using System.Collections.Generic;

namespace AbstractCafeService.BindingModel
{
    public class MenuBindingModel
    {
        public int Id { get; set; }

        public string MenuName { get; set; }

        public decimal Price { get; set; }

        public List<MenuDishBindingModel> MenuDishs { get; set; }
    }
}
