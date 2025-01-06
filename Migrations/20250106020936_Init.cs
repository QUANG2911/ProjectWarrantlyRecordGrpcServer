using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectWarrantlyRecordGrpcServer.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerDevices",
                columns: table => new
                {
                    IdDevice = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceName = table.Column<int>(type: "integer", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDevices", x => x.IdDevice);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    IdCustomer = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CustomerAddress = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CustomerEmail = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CustomerPhone = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.IdCustomer);
                });

            migrationBuilder.CreateTable(
                name: "RepairParts",
                columns: table => new
                {
                    IdRepairPart = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RepairPartName = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    DateOfManuFacture = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairParts", x => x.IdRepairPart);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    IdStaff = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StaffName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    StaffPhone = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    StaffPosition = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Pass = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.IdStaff);
                });

            migrationBuilder.CreateTable(
                name: "WarrantyRecords",
                columns: table => new
                {
                    IdWarrantRecord = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdDevice = table.Column<int>(type: "integer", nullable: false),
                    IdCustomer = table.Column<int>(type: "integer", nullable: false),
                    DateOfResig = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyRecords", x => x.IdWarrantRecord);
                    table.ForeignKey(
                        name: "FK_WarrantyRecords_CustomerDevices_IdDevice",
                        column: x => x.IdDevice,
                        principalTable: "CustomerDevices",
                        principalColumn: "IdDevice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarrantyRecords_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customers",
                        principalColumn: "IdCustomer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffTasks",
                columns: table => new
                {
                    IdTask = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdStaff = table.Column<int>(type: "integer", nullable: false),
                    IdWarantyRecord = table.Column<int>(type: "integer", nullable: false),
                    DateOfTask = table.Column<DateOnly>(type: "date", nullable: false),
                    StatusTask = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffTasks", x => x.IdTask);
                    table.ForeignKey(
                        name: "FK_StaffTasks_Staffs_IdStaff",
                        column: x => x.IdStaff,
                        principalTable: "Staffs",
                        principalColumn: "IdStaff",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffTasks_WarrantyRecords_IdWarantyRecord",
                        column: x => x.IdWarantyRecord,
                        principalTable: "WarrantyRecords",
                        principalColumn: "IdWarrantRecord",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    IdBill = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateCreateBill = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmount = table.Column<int>(type: "integer", nullable: false),
                    StatusBill = table.Column<int>(type: "integer", nullable: false),
                    IdTask = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.IdBill);
                    table.ForeignKey(
                        name: "FK_Bills_StaffTasks_IdTask",
                        column: x => x.IdTask,
                        principalTable: "StaffTasks",
                        principalColumn: "IdTask",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairDetails",
                columns: table => new
                {
                    IdRepairPart = table.Column<int>(type: "integer", nullable: false),
                    IdTask = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairDetails", x => new { x.IdRepairPart, x.IdTask });
                    table.ForeignKey(
                        name: "FK_RepairDetails_RepairParts_IdRepairPart",
                        column: x => x.IdRepairPart,
                        principalTable: "RepairParts",
                        principalColumn: "IdRepairPart",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairDetails_StaffTasks_IdTask",
                        column: x => x.IdTask,
                        principalTable: "StaffTasks",
                        principalColumn: "IdTask",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_IdTask",
                table: "Bills",
                column: "IdTask");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerEmail",
                table: "Customers",
                column: "CustomerEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairDetails_IdTask",
                table: "RepairDetails",
                column: "IdTask");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTasks_IdStaff",
                table: "StaffTasks",
                column: "IdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTasks_IdWarantyRecord",
                table: "StaffTasks",
                column: "IdWarantyRecord");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyRecords_IdCustomer",
                table: "WarrantyRecords",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyRecords_IdDevice",
                table: "WarrantyRecords",
                column: "IdDevice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "RepairDetails");

            migrationBuilder.DropTable(
                name: "RepairParts");

            migrationBuilder.DropTable(
                name: "StaffTasks");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "WarrantyRecords");

            migrationBuilder.DropTable(
                name: "CustomerDevices");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
