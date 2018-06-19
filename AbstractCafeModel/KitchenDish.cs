namespace AbstractCafeModel
{
    public class KitchenDish
    {
        public int Id { get; set; }

        public int KitchenId { get; set; }

        public int DishId { get; set; }

        public int Count { get; set; }

        public virtual Kitchen Kitchen { get; set; }

        public virtual Dish Dish { get; set; }
    }
}
