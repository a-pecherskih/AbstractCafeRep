using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractCafeModel
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string CustomerFIO { get; set; }

        public string Mail { get; set; }

        [ForeignKey("CustomerId")]
        public virtual List<Choice> Choices { get; set; }

        [ForeignKey("CustomerId")]
        public virtual List<MessageInfo> MessageInfos { get; set; }
    }
}
