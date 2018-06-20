using System;

namespace AbstractCafeModel
{
    public class Choice
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int MenuId { get; set; }

        public int? ChefId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public ChoiceStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual Chef Chef { get; set; }
    }
}
