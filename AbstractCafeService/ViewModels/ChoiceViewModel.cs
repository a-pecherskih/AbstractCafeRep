namespace AbstractCafeService.ViewModels
{
    public class ChoiceViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerFIO { get; set; }

        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public int? ChefId { get; set; }

        public string ChefName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }

        public string DateCreate { get; set; }

        public string DateImplement { get; set; }
    }
}
