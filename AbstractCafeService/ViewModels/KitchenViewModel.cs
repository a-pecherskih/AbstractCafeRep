using System.Collections.Generic;

namespace AbstractCafeService.ViewModels
{
    public class KitchenViewModel
    {
        public int Id { get; set; }

        public string KitchenName { get; set; }

        public List<KitchenDishViewModel> KitchenDishs { get; set; }
    }
}
