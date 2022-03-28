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
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220328153518_MergeAuth3")]
    partial class MergeAuth3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("vmProjectBackend.Models.AccessToken", b =>
                {
                    b.Property<int>("AccessTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("access_token_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessTokenValue")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("access_token_value");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("expire_date");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("AccessTokenId");

                    b.HasIndex("UserId");

                    b.ToTable("access_token", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("course_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentLibrary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasColumnName("course_code");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("varchar(75)")
                        .HasColumnName("course_name");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Folder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Resource_pool")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Section")
                        .HasColumnType("int");

                    b.Property<string>("Semester")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateVm")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseId");

                    b.ToTable("course", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("role_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CanvasRoleId")
                        .HasColumnType("int")
                        .HasColumnName("canvas_role_id");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId");

                    b.ToTable("role", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Section", b =>
                {
                    b.Property<int>("SectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("section_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseId")
                        .HasColumnType("int")
                        .HasColumnName("course_id");

                    b.Property<int>("SectionCanvasId")
                        .HasColumnType("int")
                        .HasColumnName("section_canvas_id");

                    b.Property<int>("SectionNumber")
                        .HasColumnType("int")
                        .HasColumnName("section_number");

                    b.Property<int>("SemesterId")
                        .HasColumnType("int")
                        .HasColumnName("semester_id");

                    b.HasKey("SectionId");

                    b.HasIndex("CourseId");

                    b.HasIndex("SemesterId");

                    b.ToTable("section", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Semester", b =>
                {
                    b.Property<int>("SemesterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("semester_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("end_date");

                    b.Property<string>("SemesterTerm")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("semester_term");

                    b.Property<int>("SemesterYear")
                        .HasColumnType("int")
                        .HasColumnName("semester_year");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("start_date");

                    b.HasKey("SemesterId");

                    b.ToTable("semester", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.SessionToken", b =>
                {
                    b.Property<int>("SessionTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("session_token_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessTokenId")
                        .HasColumnType("int")
                        .HasColumnName("access_token_id");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("expire_date");

                    b.Property<Guid>("SessionTokenValue")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("sesion_token_value");

                    b.HasKey("SessionTokenId");

                    b.HasIndex("AccessTokenId");

                    b.ToTable("session_token", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CanvasToken")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("canvas_token");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit")
                        .HasColumnName("is_admin");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("last_name");

                    b.HasKey("UserId");

                    b.ToTable("user", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.UserSectionRole", b =>
                {
                    b.Property<int>("UserSectionRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_section_role_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<int>("SectionId")
                        .HasColumnType("int")
                        .HasColumnName("section_id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("UserSectionRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("SectionId");

                    b.HasIndex("UserId");

                    b.ToTable("user_section_role", "vmProject");
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
                    b.Property<int>("VmTableID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("VmFolder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VmName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VmResourcePool")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VmTableID");

                    b.ToTable("VmTables");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTemplate", b =>
                {
                    b.Property<int>("VmTemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_template_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("VmTemplateAccessDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("vm_template_access_date");

                    b.Property<string>("VmTemplateName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vm_template_name");

                    b.Property<string>("VmTemplateVcenterId")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vm_template_vcenter_id");

                    b.HasKey("VmTemplateId");

                    b.ToTable("vm_template", "vmProject");
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

                    b.ToTable("VmUtilizations");
                });

            modelBuilder.Entity("vmProjectBackend.Models.AccessToken", b =>
                {
                    b.HasOne("vmProjectBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Section", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.Semester", "Semester")
                        .WithMany()
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Semester");
                });

            modelBuilder.Entity("vmProjectBackend.Models.SessionToken", b =>
                {
                    b.HasOne("vmProjectBackend.Models.AccessToken", "AccessToken")
                        .WithMany()
                        .HasForeignKey("AccessTokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccessToken");
                });

            modelBuilder.Entity("vmProjectBackend.Models.UserSectionRole", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Section");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
