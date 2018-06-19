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
    }
}
