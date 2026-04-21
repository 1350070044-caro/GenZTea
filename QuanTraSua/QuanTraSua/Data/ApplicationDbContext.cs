using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanTraSua.Models;

namespace QuanTraSua.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
new Product { Id = 1, Name = "Trà Sữa Trân Châu Ô Long", Price = 35000, Description = "Đậm vị trà ô long, trân châu dai giòn.", ImageUrl = "/images/matcha.jpg", Stock = 50 },
new Product { Id = 2, Name = "Trà Sữa Matcha Nhật Bản", Price = 40000, Description = "Bột matcha nguyên chất từ Nhật.", ImageUrl = "/images/matcha.jpg", Stock = 50 },
new Product { Id = 3, Name = "Sữa Tươi Trân Châu Đường Đen", Price = 45000, Description = "Sữa tươi nguyên chất mix đường đen.", ImageUrl = "/images/no-image.png", Stock = 50 }
            );
        }
    }
}
