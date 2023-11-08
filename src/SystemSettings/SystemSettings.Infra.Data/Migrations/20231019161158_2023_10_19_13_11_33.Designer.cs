﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using LazyCrud.SystemSettings.Infra.Data.Context;

#nullable disable

namespace LazyCrud.SystemSettings.Infra.Data.Migrations
{
    [DbContext(typeof(SystemSettingsAggContext))]
    [Migration("20231019161158_2023_10_19_13_11_33")]
    partial class _2023_10_19_13_11_33
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SystemPanelGroupSystemPanel", b =>
                {
                    b.Property<int>("GroupOfMenusId")
                        .HasColumnType("int");

                    b.Property<int>("SubItemsId")
                        .HasColumnType("int");

                    b.HasKey("GroupOfMenusId", "SubItemsId");

                    b.HasIndex("SubItemsId");

                    b.ToTable("SystemPanelGroupSystemPanel");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.CargaTabela", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<byte[]>("ArquivoCSV")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentStep")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInitialized")
                        .HasColumnType("bit");

                    b.Property<bool?>("RegisterDone")
                        .HasColumnType("bit");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Total")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CargaTabela");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentStep")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("RegisterDone")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SystemPanel");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentStep")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("RegisterDone")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SystemPanelGroup");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelSubItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActionButton")
                        .HasColumnType("bit");

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentStep")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSubItem")
                        .HasColumnType("bit");

                    b.Property<bool>("LinkDireto")
                        .HasColumnType("bit");

                    b.Property<bool?>("RegisterDone")
                        .HasColumnType("bit");

                    b.Property<int>("SystemPanelId")
                        .HasColumnType("int");

                    b.Property<int?>("SystemPanelSubItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SystemPanelId");

                    b.HasIndex("SystemPanelSubItemId");

                    b.ToTable("SystemPanelSubItem");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemSettingsAggSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AutoSaveSettingsEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SystemSettingsAggSettings");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.UsersAgg.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("User", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.UsersAgg.Entities.UserProfileAccess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<bool>("CanDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("CanInsert")
                        .HasColumnType("bit");

                    b.Property<bool>("CanList")
                        .HasColumnType("bit");

                    b.Property<bool>("CanUpdate")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentStep")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSubItem")
                        .HasColumnType("bit");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<bool?>("RegisterDone")
                        .HasColumnType("bit");

                    b.Property<int?>("SystemPanelGroupId")
                        .HasColumnType("int");

                    b.Property<int?>("SystemPanelId")
                        .HasColumnType("int");

                    b.Property<int?>("SystemPanelSubItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SystemPanelGroupId");

                    b.HasIndex("SystemPanelId");

                    b.ToTable("UserProfileAccess", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("SystemPanelGroupSystemPanel", b =>
                {
                    b.HasOne("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelGroup", null)
                        .WithMany()
                        .HasForeignKey("GroupOfMenusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanel", null)
                        .WithMany()
                        .HasForeignKey("SubItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelSubItem", b =>
                {
                    b.HasOne("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanel", null)
                        .WithMany("SubItems")
                        .HasForeignKey("SystemPanelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelSubItem", null)
                        .WithMany("SubItems")
                        .HasForeignKey("SystemPanelSubItemId");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.UsersAgg.Entities.UserProfileAccess", b =>
                {
                    b.HasOne("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelGroup", null)
                        .WithMany("AccessesOfMyProfile")
                        .HasForeignKey("SystemPanelGroupId");

                    b.HasOne("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanel", null)
                        .WithMany("AccessesOfMyProfile")
                        .HasForeignKey("SystemPanelId");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanel", b =>
                {
                    b.Navigation("AccessesOfMyProfile");

                    b.Navigation("SubItems");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelGroup", b =>
                {
                    b.Navigation("AccessesOfMyProfile");
                });

            modelBuilder.Entity("LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelSubItem", b =>
                {
                    b.Navigation("SubItems");
                });
#pragma warning restore 612, 618
        }
    }
}