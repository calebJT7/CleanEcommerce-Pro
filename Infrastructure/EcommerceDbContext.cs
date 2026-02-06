using Microsoft.EntityFrameworkCore;
using Domain;// <--- Importante para ver Entidades

namespace Infrastructure
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}