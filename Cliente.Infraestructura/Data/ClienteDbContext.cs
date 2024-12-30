using Cliente.Dominio.Models;
using Microsoft.EntityFrameworkCore;

namespace Cliente.Infraestructura.Data
{
    public class ClienteDbContext : DbContext
    {
        public ClienteDbContext(DbContextOptions<ClienteDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de tus entidades
            base.OnModelCreating(modelBuilder);
        }
    }
}
