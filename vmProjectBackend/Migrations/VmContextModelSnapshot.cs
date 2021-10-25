﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vmProjectBackend.DAL;

namespace vmProjectBackend.Migrations
{
    [DbContext(typeof(VmContext))]
    partial class VmContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("vmProjectBackend.Models.Course", b =>
                {
                    b.Property<long>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CourseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("canvas_token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("section_num")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("semester")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseID");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Enrollment", b =>
                {
                    b.Property<long>("EnrollmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CourseID")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("section_num")
                        .HasColumnType("int");

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
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("vmProjectBackend.Models.User", b =>
                {
                    b.Property<long>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("userAccess")
                        .HasColumnType("bit");

                    b.Property<string>("userType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTable", b =>
                {
                    b.Property<long>("VmTableID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<int>("section_num")
                        .HasColumnType("int");

                    b.Property<string>("vm_image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VmTableID");

                    b.ToTable("VmTable");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTableCourse", b =>
                {
                    b.Property<int>("VmTableCourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CourseID")
                        .HasColumnType("bigint");

                    b.Property<long>("VmTableID")
                        .HasColumnType("bigint");

                    b.HasKey("VmTableCourseID");

                    b.HasIndex("CourseID");

                    b.HasIndex("VmTableID");

                    b.ToTable("VmTableCourse");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Enrollment", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.User", "User")
                        .WithMany("Enrollments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTableCourse", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Course", "Course")
                        .WithMany("VmTableCourses")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.VmTable", "VmTable")
                        .WithMany("VmTableCourses")
                        .HasForeignKey("VmTableID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("VmTable");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Course", b =>
                {
                    b.Navigation("Enrollments");

                    b.Navigation("VmTableCourses");
                });

            modelBuilder.Entity("vmProjectBackend.Models.User", b =>
                {
                    b.Navigation("Enrollments");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTable", b =>
                {
                    b.Navigation("VmTableCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
