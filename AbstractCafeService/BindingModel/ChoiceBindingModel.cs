namespace AbstractCafeService.BindingModel
{
    public class ChoiceBindingModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int MenuId { get; set; }

        public int? ChefId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
