using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractCafeModel
{
    public class Menu
    {
        public int Id { get; set; }

        [Required]
        public string MenuName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("MenuId")]
        public virtual List<Choice> Choices { get; set; }

        [ForeignKey("MenuId")]
        public virtual List<MenuDish> MenuDishs { get; set; }
    }
}
