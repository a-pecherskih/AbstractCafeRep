using System.Collections.Generic;

namespace AbstractCafeService.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }

        public string MenuName { get; set; }

        public decimal Price { get; set; }

        public List<MenuDishViewModel> MenuDishs { get; set; }
    }
}
