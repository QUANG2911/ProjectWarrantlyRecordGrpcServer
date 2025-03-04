﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectWarrantlyRecordGrpcServer.Data;

#nullable disable

namespace ProjectWarrantlyRecordGrpcServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250106020936_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.Bill", b =>
                {
                    b.Property<int>("IdBill")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdBill"));

                    b.Property<DateOnly>("DateCreateBill")
                        .HasColumnType("date");

                    b.Property<int>("IdTask")
                        .HasColumnType("integer");

                    b.Property<int>("StatusBill")
                        .HasColumnType("integer");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("integer");

                    b.HasKey("IdBill");

                    b.HasIndex("IdTask");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.Customer", b =>
                {
                    b.Property<int>("IdCustomer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdCustomer"));

                    b.Property<string>("CustomerAddress")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("IdCustomer");

                    b.HasIndex("CustomerEmail")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.CustomerDevices", b =>
                {
                    b.Property<int>("IdDevice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdDevice"));

                    b.Property<int>("DeviceName")
                        .HasMaxLength(30)
                        .HasColumnType("integer");

                    b.HasKey("IdDevice");

                    b.ToTable("CustomerDevices");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.RepairDetail", b =>
                {
                    b.Property<int>("IdRepairPart")
                        .HasColumnType("integer");

                    b.Property<int>("IdTask")
                        .HasColumnType("integer");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.HasKey("IdRepairPart", "IdTask");

                    b.HasIndex("IdTask");

                    b.ToTable("RepairDetails");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.RepairPart", b =>
                {
                    b.Property<int>("IdRepairPart")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdRepairPart"));

                    b.Property<DateOnly>("DateOfManuFacture")
                        .HasColumnType("date");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<string>("RepairPartName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdRepairPart");

                    b.ToTable("RepairParts");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.Staff", b =>
                {
                    b.Property<int>("IdStaff")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdStaff"));

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("StaffName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("StaffPhone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("StaffPosition")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("IdStaff");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.StaffTask", b =>
                {
                    b.Property<int>("IdTask")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdTask"));

                    b.Property<DateOnly>("DateOfTask")
                        .HasColumnType("date");

                    b.Property<int>("IdStaff")
                        .HasColumnType("integer");

                    b.Property<int>("IdWarantyRecord")
                        .HasColumnType("integer");

                    b.Property<int>("StatusTask")
                        .HasColumnType("integer");

                    b.HasKey("IdTask");

                    b.HasIndex("IdStaff");

                    b.HasIndex("IdWarantyRecord");

                    b.ToTable("StaffTasks");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.WarrantyRecord", b =>
                {
                    b.Property<int>("IdWarrantRecord")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdWarrantRecord"));

                    b.Property<DateOnly>("DateOfResig")
                        .HasColumnType("date");

                    b.Property<int>("IdCustomer")
                        .HasColumnType("integer");

                    b.Property<int>("IdDevice")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("TimeEnd")
                        .HasColumnType("date");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.HasKey("IdWarrantRecord");

                    b.HasIndex("IdCustomer");

                    b.HasIndex("IdDevice");

                    b.ToTable("WarrantyRecords");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.Bill", b =>
                {
                    b.HasOne("ProjectWarrantlyRecordGrpcServer.Model.StaffTask", "staffTask")
                        .WithMany("bills")
                        .HasForeignKey("IdTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("staffTask");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.RepairDetail", b =>
                {
                    b.HasOne("ProjectWarrantlyRecordGrpcServer.Model.RepairPart", "RepairPart")
                        .WithMany("repairDetails")
                        .HasForeignKey("IdRepairPart")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectWarrantlyRecordGrpcServer.Model.StaffTask", "StaffTask")
                        .WithMany("repairDetails")
                        .HasForeignKey("IdTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RepairPart");

                    b.Navigation("StaffTask");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.StaffTask", b =>
                {
                    b.HasOne("ProjectWarrantlyRecordGrpcServer.Model.Staff", "staff")
                        .WithMany("staffTasks")
                        .HasForeignKey("IdStaff")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectWarrantlyRecordGrpcServer.Model.WarrantyRecord", "warrantyRecord")
                        .WithMany("staffTasks")
                        .HasForeignKey("IdWarantyRecord")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("staff");

                    b.Navigation("warrantyRecord");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.WarrantyRecord", b =>
                {
                    b.HasOne("ProjectWarrantlyRecordGrpcServer.Model.Customer", "Customer")
                        .WithMany("warrantyRecords")
                        .HasForeignKey("IdCustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectWarrantlyRecordGrpcServer.Model.CustomerDevices", "customerDevice")
                        .WithMany("warrantyRecords")
                        .HasForeignKey("IdDevice")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("customerDevice");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.Customer", b =>
                {
                    b.Navigation("warrantyRecords");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.CustomerDevices", b =>
                {
                    b.Navigation("warrantyRecords");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.RepairPart", b =>
                {
                    b.Navigation("repairDetails");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.Staff", b =>
                {
                    b.Navigation("staffTasks");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.StaffTask", b =>
                {
                    b.Navigation("bills");

                    b.Navigation("repairDetails");
                });

            modelBuilder.Entity("ProjectWarrantlyRecordGrpcServer.Model.WarrantyRecord", b =>
                {
                    b.Navigation("staffTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
