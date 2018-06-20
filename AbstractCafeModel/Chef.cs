using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractCafeModel
{
    public class Chef
    {
        public int Id { get; set; }

        [Required]
        public string ChefFIO { get; set; }

        [ForeignKey("ChefId")]
        public virtual List<Choice> Choices { get; set; }
    }
}
