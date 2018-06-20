namespace AbstractCafeModel
{
    public class MenuDish
    {
        public int Id { get; set; }

        public int MenuId { get; set; }

        public int DishId { get; set; }

        public int Count { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual Dish Dish { get; set; }
    }
}
