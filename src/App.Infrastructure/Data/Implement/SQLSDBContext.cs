using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure
{
    public class SQLSDBContext : DbContext
    {
        public DbSet<Tattoo> Tattoo { get; set; }
        public DbSet<ConfigValue> ConfigValue { get; set; }
        public DbSet<Reservation> Reservation { get; set; }


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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=SQL8006.site4now.net;Initial Catalog=db_aaa3b0_tattoodev;User Id=db_aaa3b0_tattoodev_admin;Password=Tatoo99@;TrustServerCertificate=true;", b => b.MigrationsAssembly("App.Infrastructure"));

            optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER22;Database=studioDB;User=sa;Password=123;TrustServerCertificate=true;", b => b.MigrationsAssembly("App.Infrastructure"));
        }
    }
}
