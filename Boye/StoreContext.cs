using Boye.Models;
using Microsoft.EntityFrameworkCore;

namespace Boye
{
    public class StoreContext: DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options): base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
    }
}
