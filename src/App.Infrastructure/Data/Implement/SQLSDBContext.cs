using App.Domain.Entities;
using App.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure
{
    public class SQLSDBContext : DbContext
    {
        public DbSet<Tattoo> Tattoo { get; set; }
        public DbSet<ConfigValue> ConfigValue { get; set; }
        public DbSet<Reservation> Reservation { get; set; }

        //**************Inventary*************
        public DbSet<Product> Products { get; set; }
        public DbSet<FixedAsset> FixedAssets { get; set; }
        public DbSet<Merchandise> Merchandises { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        //public SQLSDBContext() { }
        public SQLSDBContext(DbContextOptions<SQLSDBContext> options) : base(options) { }
        public SQLSDBContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Relación entre Tattoo y Reservation (Uno a Muchos)
            modelBuilder.Entity<Tattoo>()
            .HasOne(t => t.TattooStyle)
            .WithMany()
            .HasForeignKey(t => t.TattooStyleId);

            modelBuilder.Entity<Tattoo>()
                .HasOne(t => t.TattooBodyPart)
                .WithMany()
                .HasForeignKey(t => t.TattooBodyPartId);

            modelBuilder.Entity<Tattoo>()
                .HasOne(t => t.TattooGenre)
                .WithMany()
                .HasForeignKey(t => t.TattooGenreId);


            //*********Inventary*************
            // Configuración para Product
            modelBuilder.Entity<Product>()
                .HasDiscriminator(p => p.ProductType)
                .HasValue<FixedAsset>(ProductType.FixedAsset)
                .HasValue<Merchandise>(ProductType.Merchandise)
                .HasValue<RawMaterial>(ProductType.RawMaterial);

            // Configuración para FixedAsset
            modelBuilder.Entity<FixedAsset>()
                .HasOne(fa => fa.Product)
                .WithOne(p => p.FixedAsset)
                .HasForeignKey<FixedAsset>(fa => fa.ProductId);

            // Configuración para Merchandise
            modelBuilder.Entity<Merchandise>()
                .HasOne(m => m.Product)
                .WithOne(p => p.Merchandise)
                .HasForeignKey<Merchandise>(m => m.ProductId);

            // Configuración para RawMaterial
            modelBuilder.Entity<RawMaterial>()
                .HasOne(rm => rm.Product)
                .WithOne(p => p.RawMaterial)
                .HasForeignKey<RawMaterial>(rm => rm.ProductId);

            // Configuración para Reservation
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Transactions)
                .WithOne(t => t.Reservation)
                .HasForeignKey(t => t.ReservationId);

            // Configuración para Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Reservation)
                .WithMany(r => r.Transactions)
                .HasForeignKey(t => t.ReservationId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=SQL8006.site4now.net;Initial Catalog=db_aaa3b0_tattoodev;User Id=db_aaa3b0_tattoodev_admin;Password=Tatoo99@;TrustServerCertificate=true;", b => b.MigrationsAssembly("App.Infrastructure"));

            optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER22;Database=studioDB;User=sa;Password=123;TrustServerCertificate=true;", b => b.MigrationsAssembly("App.Infrastructure"));
        }
    }
}
