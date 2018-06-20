using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractCafeModel
{
    public class Dish
    {
        public int Id { get; set; }

        [Required]
        public string DishName { get; set; }

        [ForeignKey("DishId")]
        public virtual List<MenuDish> MenuDishs { get; set; }

        [ForeignKey("DishId")]
        public virtual List<KitchenDish> KitchenDishs { get; set; }
    }
}
