using App.Domain.Entities;
using App.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure
{
    public class SQLSDBContext : DbContext
    {
        public DbSet<Tattoo> Tattoo { get; set; }
        public DbSet<ConfigValue> ConfigValue { get; set; }
        public DbSet<Reservation> Reservation { get; set; }

        //**************Inventario*************
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        private readonly IConfiguration _configuration;
        public SQLSDBContext(DbContextOptions<SQLSDBContext> options, IConfiguration configuration) : base(options) { _configuration = configuration; }

        public SQLSDBContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //********* Relación entre Tattoo y Reservation (Uno a Muchos) *********
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

            //********* Configuración del Inventario *********
            // Configuración para Product y herencia TPH (Table Per Hierarchy)
            modelBuilder.Entity<Product>()
                .HasDiscriminator<string>("ProductTypeDiscriminator")
                .HasValue<Product>("Product")
                .HasValue<Merchandise>("Merchandise")
                .HasValue<FixedAsset>("FixedAsset")
                .HasValue<RawMaterial>("RawMaterial");

            // Relación de Reservation con Transaction
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Transactions)
                .WithOne(t => t.Reservation)
                .HasForeignKey(t => t.ReservationId);

            // Configuración de Transaction relacionada con Product y Reservation
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
            var connectionString = _configuration.GetConnectionString("SQLServerConnection");

            // Configuración de la base de datos (cambiar según sea necesario)
            //optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("App.Infrastructure"));
            optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER22;Database=studioDB;User=sa;Password=123;TrustServerCertificate=true;", b => b.MigrationsAssembly("App.Infrastructure"));
        }
    }
}
