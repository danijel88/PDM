﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PDM.Data;
using PDM.Models;
using System;

namespace PDM.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PDM.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Company");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PDM.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Band");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .HasMaxLength(4096);

                    b.Property<bool>("Elastic");

                    b.Property<decimal>("Enter");

                    b.Property<decimal>("Exit");

                    b.Property<string>("ImagePath");

                    b.Property<string>("InternalCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("ItemTypeId");

                    b.Property<int>("MachineTypeId");

                    b.Property<string>("MadeBy")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Status");

                    b.Property<decimal>("Thickness");

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("InternalCode")
                        .IsUnique();

                    b.HasIndex("ItemTypeId");

                    b.HasIndex("MachineTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PDM.Models.ItemHist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Band");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .HasMaxLength(4096);

                    b.Property<bool>("Elastic");

                    b.Property<decimal>("Enter");

                    b.Property<decimal>("Exit");

                    b.Property<string>("InternalCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("ItemId");

                    b.Property<int>("ItemTypeId");

                    b.Property<int>("MachineTypeId");

                    b.Property<string>("MadeBy")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Status");

                    b.Property<decimal>("Thickness");

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ItemTypeId");

                    b.HasIndex("MachineTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("ItemHists");
                });

            modelBuilder.Entity("PDM.Models.ItemImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .HasMaxLength(4096);

                    b.Property<bool>("Download");

                    b.Property<int>("ItemId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("ItemImages");
                });

            modelBuilder.Entity("PDM.Models.ItemType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .HasMaxLength(4096);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("ItemTypes");
                });

            modelBuilder.Entity("PDM.Models.MachineType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .HasMaxLength(4096);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("MachineTypes");
                });

            modelBuilder.Entity("PDM.Models.Pdm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .HasMaxLength(4096);

                    b.Property<int>("ItemId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Pdm");
                });

            modelBuilder.Entity("PDM.Models.Proposal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Band");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .HasMaxLength(4096);

                    b.Property<bool>("Elastic");

                    b.Property<decimal>("Enter");

                    b.Property<decimal>("Exit");

                    b.Property<int>("ItemId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Notes");

                    b.Property<int>("Status");

                    b.Property<decimal>("Thickness");

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Propospals");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PDM.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PDM.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PDM.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PDM.Models.Item", b =>
                {
                    b.HasOne("PDM.Models.ItemType", "ItemType")
                        .WithMany()
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.MachineType", "MachineType")
                        .WithMany()
                        .HasForeignKey("MachineTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PDM.Models.ItemHist", b =>
                {
                    b.HasOne("PDM.Models.Item", "Item")
                        .WithMany("History")
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PDM.Models.ItemType", "ItemType")
                        .WithMany()
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.MachineType", "MachineType")
                        .WithMany()
                        .HasForeignKey("MachineTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PDM.Models.ItemImage", b =>
                {
                    b.HasOne("PDM.Models.Item", "Item")
                        .WithMany("Images")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PDM.Models.ItemType", b =>
                {
                    b.HasOne("PDM.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PDM.Models.MachineType", b =>
                {
                    b.HasOne("PDM.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PDM.Models.Pdm", b =>
                {
                    b.HasOne("PDM.Models.Item", "Item")
                        .WithMany("Pdms")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PDM.Models.Proposal", b =>
                {
                    b.HasOne("PDM.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDM.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
