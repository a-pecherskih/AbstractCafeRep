using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractCafeModel
{
    public class Kitchen
    {
        public int Id { get; set; }

        [Required]
        public string KitchenName { get; set; }

        [ForeignKey("KitchenId")]
        public virtual List<KitchenDish> KitchenDishs { get; set; }
    }
}
