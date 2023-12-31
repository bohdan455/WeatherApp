﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherApp.DataAccess;

#nullable disable

namespace WeatherApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230920090500_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WeatherApp.DataAccess.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("WeatherApp.DataAccess.Entities.GeoInfromation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Lat")
                        .HasColumnType("float");

                    b.Property<double>("Lon")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("GeoInfromation");
                });

            modelBuilder.Entity("WeatherApp.DataAccess.Entities.WeatherInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("DefaultLocationName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("FeelsLikeTemperature")
                        .HasColumnType("float");

                    b.Property<int>("GeoInfromationId")
                        .HasColumnType("int");

                    b.Property<int>("Humidity")
                        .HasColumnType("int");

                    b.Property<double>("MaximumTemperature")
                        .HasColumnType("float");

                    b.Property<double>("MinimumTemperature")
                        .HasColumnType("float");

                    b.Property<int>("Pressure")
                        .HasColumnType("int");

                    b.Property<double>("RainVolume")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.Property<double>("WindSpeed")
                        .HasColumnType("float");

                    b.Property<int?>("ZipCodeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CityId")
                        .IsUnique();

                    b.HasIndex("GeoInfromationId");

                    b.HasIndex("ZipCodeId")
                        .IsUnique()
                        .HasFilter("[ZipCodeId] IS NOT NULL");

                    b.ToTable("WeatherInformations");
                });

            modelBuilder.Entity("WeatherApp.DataAccess.Entities.ZipCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("ZipCodes");
                });

            modelBuilder.Entity("WeatherApp.DataAccess.Entities.WeatherInformation", b =>
                {
                    b.HasOne("WeatherApp.DataAccess.Entities.City", "City")
                        .WithOne("WeatherInformation")
                        .HasForeignKey("WeatherApp.DataAccess.Entities.WeatherInformation", "CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeatherApp.DataAccess.Entities.GeoInfromation", "GeoInfromation")
                        .WithMany()
                        .HasForeignKey("GeoInfromationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeatherApp.DataAccess.Entities.ZipCode", "ZipCode")
                        .WithOne("WeatherInformation")
                        .HasForeignKey("WeatherApp.DataAccess.Entities.WeatherInformation", "ZipCodeId");

                    b.Navigation("City");

                    b.Navigation("GeoInfromation");

                    b.Navigation("ZipCode");
                });

            modelBuilder.Entity("WeatherApp.DataAccess.Entities.City", b =>
                {
                    b.Navigation("WeatherInformation")
                        .IsRequired();
                });

            modelBuilder.Entity("WeatherApp.DataAccess.Entities.ZipCode", b =>
                {
                    b.Navigation("WeatherInformation")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
