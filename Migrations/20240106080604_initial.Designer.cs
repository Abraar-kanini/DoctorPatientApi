﻿// <auto-generated />
using System;
using DoctorPatient.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DoctorPatient.Migrations
{
    [DbContext(typeof(DoctorPatientDbContext))]
    [Migration("20240106080604_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DoctorPatient.model.Doctor", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FileSizeInBytes")
                        .HasColumnType("bigint");

                    b.Property<string>("doctorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("doctorSpecialist")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("doctors");
                });

            modelBuilder.Entity("DoctorPatient.model.Patients", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("patientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("DoctorId");

                    b.ToTable("patients");
                });

            modelBuilder.Entity("DoctorPatient.model.Patients", b =>
                {
                    b.HasOne("DoctorPatient.model.Doctor", "doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("doctor");
                });
#pragma warning restore 612, 618
        }
    }
}
