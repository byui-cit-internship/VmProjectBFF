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
    [Migration("20220217083857_FixTemplate")]
    partial class FixTemplate
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

            modelBuilder.Entity("vmProjectBackend.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("group_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CanvasGroupId")
                        .HasColumnType("int")
                        .HasColumnName("canvas_group_id");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("varchar(45)")
                        .HasColumnName("group_name");

                    b.Property<int>("SectionId")
                        .HasColumnType("int")
                        .HasColumnName("section_id");

                    b.HasKey("GroupId");

                    b.HasIndex("SectionId");

                    b.ToTable("group", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.GroupMembership", b =>
                {
                    b.Property<int>("GroupMembershipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("group_membership_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("GroupMembershipId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("group_membership", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.IpAddress", b =>
                {
                    b.Property<int>("IpAddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ip_address_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("IpAddressField")
                        .IsRequired()
                        .HasColumnType("binary(16)")
                        .HasColumnName("ip_address");

                    b.Property<bool>("IsIpv6")
                        .HasColumnType("bit")
                        .HasColumnName("is_ipv6");

                    b.Property<byte[]>("SubnetMask")
                        .IsRequired()
                        .HasColumnType("binary(16)")
                        .HasColumnName("subnet_mask");

                    b.HasKey("IpAddressId");

                    b.ToTable("ip_address", "vmProject");
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

            modelBuilder.Entity("vmProjectBackend.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("tag_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TagCategoryId")
                        .HasColumnType("int")
                        .HasColumnName("tag_category_id");

                    b.Property<string>("TagDescription")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tag_description");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("varchar(45)")
                        .HasColumnName("tag_name");

                    b.Property<string>("TagVcenterId")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tag_vcenter_id");

                    b.HasKey("TagId");

                    b.HasIndex("TagCategoryId");

                    b.ToTable("tag", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.TagCategory", b =>
                {
                    b.Property<int>("TagCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("tag_category_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TagCategoryDescription")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tag_category_description");

                    b.Property<string>("TagCategoryName")
                        .IsRequired()
                        .HasColumnType("varchar(45)")
                        .HasColumnName("tag_category_name");

                    b.Property<string>("TagCategoryVcenterId")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tag_category_vcenter_id");

                    b.HasKey("TagCategoryId");

                    b.ToTable("tag_category", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.TagUser", b =>
                {
                    b.Property<int>("TagUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("tag)_user_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasColumnName("tag_id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("TagUserId");

                    b.HasIndex("TagId");

                    b.HasIndex("UserId");

                    b.ToTable("tag_user", "vmProject");
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

            modelBuilder.Entity("vmProjectBackend.Models.Vlan", b =>
                {
                    b.Property<int>("VlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vlan_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("VlanDescription")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("vlan_description");

                    b.Property<int>("VlanNumber")
                        .HasColumnType("int")
                        .HasColumnName("vlan_number");

                    b.HasKey("VlanId");

                    b.ToTable("vlan", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VlanVswitch", b =>
                {
                    b.Property<int>("VlanVswitchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vlan_vswitch_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("VlanId")
                        .HasColumnType("int")
                        .HasColumnName("vlan_id");

                    b.Property<int>("VswitchId")
                        .HasColumnType("int")
                        .HasColumnName("vswitch_id");

                    b.HasKey("VlanVswitchId");

                    b.HasIndex("VlanId");

                    b.HasIndex("VswitchId");

                    b.ToTable("vlan_vswitch", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstance", b =>
                {
                    b.Property<int>("VmUserInstanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_instance_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("VmTemplateId")
                        .HasColumnType("int")
                        .HasColumnName("vm_template_id");

                    b.Property<string>("VmUserInstanceVcenterId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vm_instance_vcenter_id");

                    b.Property<DateTime>("vm_user_instance_expire_date")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("vm_instance_expire_date");

                    b.HasKey("VmUserInstanceId");

                    b.HasIndex("VmTemplateId");

                    b.ToTable("vm_instance", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstanceIpAddress", b =>
                {
                    b.Property<int>("VmInstanceIpAddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_instance_ip_address_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IpAddressId")
                        .HasColumnType("int")
                        .HasColumnName("ip_address_id");

                    b.Property<int>("VmInstanceId")
                        .HasColumnType("int")
                        .HasColumnName("vm_instance_id");

                    b.HasKey("VmInstanceIpAddressId");

                    b.HasIndex("IpAddressId");

                    b.HasIndex("VmInstanceId");

                    b.ToTable("vm_instance_ip_address", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstanceTag", b =>
                {
                    b.Property<int>("VmInstanceTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_instance_tag_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasColumnName("tag_id");

                    b.Property<int>("VmInstanceId")
                        .HasColumnType("int")
                        .HasColumnName("vm_instance_id");

                    b.HasKey("VmInstanceTagId");

                    b.HasIndex("TagId");

                    b.HasIndex("VmInstanceId");

                    b.ToTable("vm_instance_tag", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstanceVswitch", b =>
                {
                    b.Property<int>("VmInstanceVswitchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_instance_vswitch_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("VmInstanceId")
                        .HasColumnType("int")
                        .HasColumnName("vm_instance_id");

                    b.Property<int>("VswitchId")
                        .HasColumnType("int")
                        .HasColumnName("vswitch_id");

                    b.HasKey("VmInstanceVswitchId");

                    b.HasIndex("VmInstanceId");

                    b.HasIndex("VswitchId");

                    b.ToTable("vm_instance_vswitch", "vmProject");
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
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vm_template_vcenter_id");

                    b.HasKey("VmTemplateId");

                    b.ToTable("vm_template", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTemplateTag", b =>
                {
                    b.Property<int>("VmTemplateTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vm_template_tag_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasColumnName("tag_id");

                    b.Property<int>("VmTemplateId")
                        .HasColumnType("int")
                        .HasColumnName("vm_template_id");

                    b.HasKey("VmTemplateTagId");

                    b.HasIndex("TagId");

                    b.HasIndex("VmTemplateId");

                    b.ToTable("vm_template_tag", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Vswitch", b =>
                {
                    b.Property<int>("VswitchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vswitch_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("VswitchDescription")
                        .IsRequired()
                        .HasColumnType("varchar(45)")
                        .HasColumnName("vswitch_description");

                    b.Property<string>("VswitchName")
                        .IsRequired()
                        .HasColumnType("varchar(45)")
                        .HasColumnName("vswitch_name");

                    b.HasKey("VswitchId");

                    b.ToTable("vswitch", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VswitchTag", b =>
                {
                    b.Property<int>("VswitchTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("vswitch_tag_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasColumnName("tag_id");

                    b.Property<int>("VswitchId")
                        .HasColumnType("int")
                        .HasColumnName("vswitch_id");

                    b.HasKey("VswitchTagId");

                    b.HasIndex("TagId");

                    b.HasIndex("VswitchId");

                    b.ToTable("vswitch_tag", "vmProject");
                });

            modelBuilder.Entity("vmProjectBackend.Models.Group", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Section");
                });

            modelBuilder.Entity("vmProjectBackend.Models.GroupMembership", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

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

            modelBuilder.Entity("vmProjectBackend.Models.Tag", b =>
                {
                    b.HasOne("vmProjectBackend.Models.TagCategory", "TagCategory")
                        .WithMany()
                        .HasForeignKey("TagCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TagCategory");
                });

            modelBuilder.Entity("vmProjectBackend.Models.TagUser", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("User");
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

            modelBuilder.Entity("vmProjectBackend.Models.VlanVswitch", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Vlan", "Vlan")
                        .WithMany()
                        .HasForeignKey("VlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.Vswitch", "Vswitch")
                        .WithMany()
                        .HasForeignKey("VswitchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vlan");

                    b.Navigation("Vswitch");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstance", b =>
                {
                    b.HasOne("vmProjectBackend.Models.VmTemplate", "VmTemplate")
                        .WithMany()
                        .HasForeignKey("VmTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VmTemplate");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstanceIpAddress", b =>
                {
                    b.HasOne("vmProjectBackend.Models.IpAddress", "IpAddress")
                        .WithMany()
                        .HasForeignKey("IpAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.VmInstance", "VmInstance")
                        .WithMany()
                        .HasForeignKey("VmInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IpAddress");

                    b.Navigation("VmInstance");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstanceTag", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.VmInstance", "VmInstance")
                        .WithMany()
                        .HasForeignKey("VmInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("VmInstance");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmInstanceVswitch", b =>
                {
                    b.HasOne("vmProjectBackend.Models.VmInstance", "VmInstance")
                        .WithMany()
                        .HasForeignKey("VmInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.Vswitch", "Vswitch")
                        .WithMany()
                        .HasForeignKey("VswitchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VmInstance");

                    b.Navigation("Vswitch");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VmTemplateTag", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.VmTemplate", "VmTemplate")
                        .WithMany()
                        .HasForeignKey("VmTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("VmTemplate");
                });

            modelBuilder.Entity("vmProjectBackend.Models.VswitchTag", b =>
                {
                    b.HasOne("vmProjectBackend.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vmProjectBackend.Models.Vswitch", "Vswitch")
                        .WithMany()
                        .HasForeignKey("VswitchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("Vswitch");
                });
#pragma warning restore 612, 618
        }
    }
}