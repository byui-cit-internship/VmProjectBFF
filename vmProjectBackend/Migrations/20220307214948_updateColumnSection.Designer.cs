﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vmProjectBackend.DAL;

namespace vmProjectBackend.Migrations
{
    [DbContext(typeof(VmContext))]
    [Migration("20220307214948_updateColumnSection")]
    partial class updateColumnSection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("vmProjectBackend.Models.Course", b =>
                {
                    b.Property<long>("CourseID")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentLibrary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Section")
                        .HasColumnType("int");

                    b.Property<string>("Semester")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateVm")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseID");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Enrollment", b =>
                {
                    b.Property<Guid>("EnrollmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CourseID")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VmTableID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("canvas_token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("semester")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("teacherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EnrollmentID");

                    b.HasIndex("CourseID");

                    b.HasIndex("UserId");

                    b.ToTable("Enrollment");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Token", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("vmProjectBackend.Models.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("userAccess")
                        .HasColumnType("bit");

                    b.Property<string>("userType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmDetail", b =>
                {
                    b.Property<Guid>("VmDetailsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Course_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Enrollment_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Template_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User_id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VmDetailsID");

                    b.ToTable("VmDetails");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTable", b =>
                {
                    b.Property<Guid>("VmTableID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("vm_image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VmTableID");

                    b.ToTable("VmTable");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmUtilization", b =>
                {
                    b.Property<Guid>("UtilizationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StudentEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VirtualMachine")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UtilizationID");

                    b.ToTable("VmUtilization");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Enrollment", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
