using System.Reflection;
using Core.Entites;
using Core.Entites.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext:DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {

        }
        public DbSet<Product> Products{set;get;}
        public DbSet<ProductBrand> ProductBrands{set;get;}
        public DbSet<ProductType> ProductTYpes{set;get;}
        public DbSet<Order> Orders{set;get;}
        public DbSet<OrderItem> OrderItems{set;get;}
        public DbSet<DeliveryMethod> DeliveryMethods{set;get;}



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if(Database.ProviderName=="Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties=entityType.ClrType.GetProperties().Where(p=>p.PropertyType==typeof(decimal));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion<double>();
                    }
                    
                }
            }
        }

    }
}