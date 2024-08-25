﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.Infrastructure.Migrations
{
    [DbContext(typeof(SQLSDBContext))]
    [Migration("20240613024150_reservation_status_SetToType")]
    partial class reservation_status_SetToType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("App.Domain.Entities.ConfigValue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ValueType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ConfigValue");
                });

            modelBuilder.Entity("App.Domain.Entities.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("CurrentStateType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("TattooId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TattooId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("App.Domain.Entities.Tattoo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TattooBodyPartId")
                        .HasColumnType("bigint");

                    b.Property<string>("TattooDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TattooGenreId")
                        .HasColumnType("bigint");

                    b.Property<string>("TattooName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TattooStyleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TattooBodyPartId");

                    b.HasIndex("TattooGenreId");

                    b.HasIndex("TattooStyleId");

                    b.ToTable("Tattoo");
                });

            modelBuilder.Entity("App.Domain.Entities.Reservation", b =>
                {
                    b.HasOne("App.Domain.Entities.Tattoo", "Tattoo")
                        .WithMany("Reservations")
                        .HasForeignKey("TattooId");

                    b.Navigation("Tattoo");
                });

            modelBuilder.Entity("App.Domain.Entities.Tattoo", b =>
                {
                    b.HasOne("App.Domain.Entities.ConfigValue", "TattooBodyPart")
                        .WithMany()
                        .HasForeignKey("TattooBodyPartId");

                    b.HasOne("App.Domain.Entities.ConfigValue", "TattooGenre")
                        .WithMany()
                        .HasForeignKey("TattooGenreId");

                    b.HasOne("App.Domain.Entities.ConfigValue", "TattooStyle")
                        .WithMany()
                        .HasForeignKey("TattooStyleId");

                    b.Navigation("TattooBodyPart");

                    b.Navigation("TattooGenre");

                    b.Navigation("TattooStyle");
                });

            modelBuilder.Entity("App.Domain.Entities.Tattoo", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
