using LicentaFacturare.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LicentaFacturare.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Partener> Parteneri { get; set; }
        public DbSet<Produs> Produse { get; set; }
        public DbSet<Factura> Facturi { get; set; }
        public DbSet<LinieFactura> LiniiFactura { get; set; }
        public DbSet<Plata> Plati { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Factura>()
                .HasOne(f => f.Partener)
                .WithMany(p => p.Facturi)
                .HasForeignKey(f => f.PartenerId);

            builder.Entity<Produs>()
                .Property(p => p.Pret)
                .HasPrecision(18, 2);

            builder.Entity<LinieFactura>()
                .Property(l => l.Cantitate)
                .HasPrecision(18, 2);

            builder.Entity<LinieFactura>()
                .Property(l => l.PretUnitarFaraTVA)
                .HasPrecision(18, 2);

            builder.Entity<LinieFactura>()
                .Property(l => l.PretUnitarCuTVA)
                .HasPrecision(18, 2);

            builder.Entity<LinieFactura>()
                .Property(l => l.Valoare)
                .HasPrecision(18, 2);

            builder.Entity<LinieFactura>()
                .Property(l => l.ValoareTVA)
                .HasPrecision(18, 2);

            builder.Entity<LinieFactura>()
                .Property(l => l.TotalLinie)
                .HasPrecision(18, 2);

            builder.Entity<Plata>()
                .HasOne(p => p.Factura)
                .WithMany(f => f.Plati)
                .HasForeignKey(p => p.FacturaId);

            builder.Entity<Plata>()
                .Property(p => p.Suma)
                .HasPrecision(18, 2);

            builder.Entity<Factura>()
                .HasIndex(f => new { f.Serie, f.Numar })
                .IsUnique();
        }
    }
}