using AbstractCafeModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AbstractCafeService
{
    [Table("AbstractDataBase")]
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext()
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Dish> Dishs { get; set; }

        public virtual DbSet<Chef> Chefs { get; set; }

        public virtual DbSet<Choice> Choices { get; set; }

        public virtual DbSet<Menu> Menus { get; set; }

        public virtual DbSet<MenuDish> MenuDishs { get; set; }

        public virtual DbSet<Kitchen> Kitchens { get; set; }

        public virtual DbSet<KitchenDish> KitchenDishs { get; set; }
    }
}
