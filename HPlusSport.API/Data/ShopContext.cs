using HPlusSport.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.API.Data
{
    public class ShopContext:DbContext
    {
        private ShopContext _context;
        public ShopContext(DbContextOptions<ShopContext> options):base(options) 
        {
            
         }

        //Set model builder, one category has many products, one product has one category, foreignkey is categoryID
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Seed();
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
    
}
