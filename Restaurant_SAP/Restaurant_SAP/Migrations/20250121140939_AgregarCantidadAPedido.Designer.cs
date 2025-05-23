﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant_SAP.DB;

#nullable disable

namespace Restaurant_SAP.Migrations
{
    [DbContext(typeof(RestauranteContext))]
    [Migration("20250121140939_AgregarCantidadAPedido")]
    partial class AgregarCantidadAPedido
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Restaurant_SAP.Models.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Restaurant_SAP.Models.Mesa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Mesas");
                });

            modelBuilder.Entity("Restaurant_SAP.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("datetime2");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<int>("MesaId")
                        .HasColumnType("int");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.HasIndex("MesaId");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("Restaurant_SAP.Models.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaHoraFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaHoraInicio")
                        .HasColumnType("datetime2");

                    b.Property<int>("MesaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MesaId");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("Restaurant_SAP.Models.Pedido", b =>
                {
                    b.HasOne("Restaurant_SAP.Models.Menu", "Menu")
                        .WithMany("Pedidos")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurant_SAP.Models.Mesa", "Mesa")
                        .WithMany("Pedidos")
                        .HasForeignKey("MesaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");

                    b.Navigation("Mesa");
                });

            modelBuilder.Entity("Restaurant_SAP.Models.Reserva", b =>
                {
                    b.HasOne("Restaurant_SAP.Models.Mesa", "Mesa")
                        .WithMany("Reservas")
                        .HasForeignKey("MesaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mesa");
                });

            modelBuilder.Entity("Restaurant_SAP.Models.Menu", b =>
                {
                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("Restaurant_SAP.Models.Mesa", b =>
                {
                    b.Navigation("Pedidos");

                    b.Navigation("Reservas");
                });
#pragma warning restore 612, 618
        }
    }
}
