﻿// <auto-generated />
using System;
using App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.Infrastructure.Migrations
{
    [DbContext(typeof(SQLSDBContext))]
    partial class SQLSDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<bool>("IsSpecialValue")
                        .HasColumnType("bit");

                    b.Property<decimal>("PriceValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ValueDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ValueType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ConfigValue");
                });

            modelBuilder.Entity("App.Domain.Entities.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FixedAssetId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MerchandiseId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<string>("ProductTypeDiscriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<decimal>("PurchaseCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FixedAssetId");

                    b.HasIndex("MerchandiseId");

                    b.ToTable("Products");

                    b.HasDiscriminator<string>("ProductTypeDiscriminator").HasValue("Product");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("App.Domain.Entities.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("BudgetAmount")
                        .HasColumnType("int");

                    b.Property<string>("ClientEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentStateType")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePathsJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCoverUp")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWithColor")
                        .HasColumnType("bit");

                    b.Property<string>("Lang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Notified")
                        .HasColumnType("bit");

                    b.Property<string>("OffersJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReservationDateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReservationDateStart")
                        .HasColumnType("datetime2");

                    b.Property<long?>("TattooId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

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

                    b.Property<string>("MiniatureImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

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

            modelBuilder.Entity("App.Domain.Entities.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<long?>("ReservationId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ReservationId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("App.Domain.Entities.FixedAsset", b =>
                {
                    b.HasBaseType("App.Domain.Entities.Product");

                    b.Property<decimal?>("DepreciationRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WarrantyExpiryDate")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("FixedAsset");
                });

            modelBuilder.Entity("App.Domain.Entities.Merchandise", b =>
                {
                    b.HasBaseType("App.Domain.Entities.Product");

                    b.Property<DateTime?>("LastRestockedDate")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("Merchandise");
                });

            modelBuilder.Entity("App.Domain.Entities.Product", b =>
                {
                    b.HasOne("App.Domain.Entities.FixedAsset", "FixedAsset")
                        .WithMany()
                        .HasForeignKey("FixedAssetId");

                    b.HasOne("App.Domain.Entities.Merchandise", "Merchandise")
                        .WithMany()
                        .HasForeignKey("MerchandiseId");

                    b.Navigation("FixedAsset");

                    b.Navigation("Merchandise");
                });

            modelBuilder.Entity("App.Domain.Entities.Reservation", b =>
                {
                    b.HasOne("App.Domain.Entities.Tattoo", null)
                        .WithMany("Reservations")
                        .HasForeignKey("TattooId");
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

            modelBuilder.Entity("App.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("App.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("App.Domain.Entities.Reservation", "Reservation")
                        .WithMany("Transactions")
                        .HasForeignKey("ReservationId");

                    b.Navigation("Product");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("App.Domain.Entities.Reservation", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("App.Domain.Entities.Tattoo", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
