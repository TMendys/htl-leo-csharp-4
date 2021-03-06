﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VaccinateRegistration.Data;

namespace VaccinateRegistration.Migrations
{
    [DbContext(typeof(VaccinateDbContext))]
    [Migration("20210307152755_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("VaccinateRegistration.Data.Registration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("BookedVaccinationId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PinCode")
                        .HasColumnType("int");

                    b.Property<long>("SocialSecurityNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("VaccinationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookedVaccinationId");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("VaccinateRegistration.Data.Vaccination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("RegistrationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("VaccinationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Vaccinations");
                });

            modelBuilder.Entity("VaccinateRegistration.Data.Registration", b =>
                {
                    b.HasOne("VaccinateRegistration.Data.Vaccination", "BookedVaccination")
                        .WithMany()
                        .HasForeignKey("BookedVaccinationId");

                    b.Navigation("BookedVaccination");
                });
#pragma warning restore 612, 618
        }
    }
}
