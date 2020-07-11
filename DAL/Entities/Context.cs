using System.Data.Entity;

namespace DAL.Entities
{
    public class Context : DbContext
    {
        public Context() : base("name = arbaDB") { }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
