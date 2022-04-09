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
    [Migration("20220121044348_InitialCreate")]
    partial class InitialCreate
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
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("course_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasColumnName("course_code");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("varchar(75)")
                        .HasColumnName("course_name");

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

            modelBuilder.Entity("vmProjectBackend.Models.VmTemplate", b =>
                {
                    b.Property<int>("VmTemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_template_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("VmTempalteAccessDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("vm_template_access_date");

                    b.Property<string>("VmTemplateName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vm_template_name");

                    b.Property<string>("VmTemplateVcenterId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vm_template_vcenter_id");

                    b.HasKey("VmTemplateId");

                    b.ToTable("vm_template", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTemplateCourse", b =>
                {
                    b.Property<int>("VmTemplateCourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_template_course_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("VmCourseId")
                        .HasColumnType("int")
                        .HasColumnName("vm_course_id");

                    b.Property<int>("VmTemplateId")
                        .HasColumnType("int")
                        .HasColumnName("vm_template_id");

                    b.HasKey("VmTemplateCourseId");

                    b.HasIndex("VmCourseId");

                    b.HasIndex("VmTemplateId");

                    b.ToTable("vm_template_course", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmUserInstance", b =>
                {
                    b.Property<int>("VmUserInstanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_user_instance_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("UserSectionRoleId")
                        .HasColumnType("int")
                        .HasColumnName("user_section_role_id");

                    b.Property<int>("VmTemplateId")
                        .HasColumnType("int")
                        .HasColumnName("vm_template_id");

                    b.Property<string>("VmUserInstanceVcenterId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vm_user_instance_vcenter_id");

                    b.Property<DateTime>("vm_user_instance_expire_date")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("vm_user_instance_expire_date");

                    b.HasKey("VmUserInstanceId");

                    b.HasIndex("UserSectionRoleId");

                    b.HasIndex("VmTemplateId");

                    b.ToTable("vm_user_instance", "vmProject");
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

            modelBuilder.Entity("vmProjectBackend.Models.VmTemplateCourse", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("VmCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.VmTemplate", "VmTemplate")
                        .WithMany()
                        .HasForeignKey("VmTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("VmTemplate");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmUserInstance", b =>
                {
                    b.HasOne("vmProjectBackend.Models.UserSectionRole", "UserSectionRole")
                        .WithMany()
                        .HasForeignKey("UserSectionRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.VmTemplate", "VmTemplate")
                        .WithMany()
                        .HasForeignKey("VmTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserSectionRole");

                    b.Navigation("VmTemplate");
                });
#pragma warning restore 612, 618
        }
    }
}
