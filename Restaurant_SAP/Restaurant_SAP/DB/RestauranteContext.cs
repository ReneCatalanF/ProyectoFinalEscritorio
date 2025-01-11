using Microsoft.EntityFrameworkCore;
using Restaurant_SAP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_SAP.DB
{
    public class RestauranteContext : DbContext
    {
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TallerRestauranteDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mesa>()
                .HasKey(m => m.Id); //Configura Id como clave primaria

            modelBuilder.Entity<Pedido>()
                    .Property(p => p.Precio)
                    .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Menu>()
                    .Property(p => p.Precio)
                    .HasColumnType("decimal(18,2)");
        }
    }
}
