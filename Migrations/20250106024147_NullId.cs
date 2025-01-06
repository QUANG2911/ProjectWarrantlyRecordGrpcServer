using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWarrantlyRecordGrpcServer.Migrations
{
    /// <inheritdoc />
    public partial class NullId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_StaffTasks_IdTask",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffTasks_Staffs_IdStaff",
                table: "StaffTasks");

            migrationBuilder.AlterColumn<int>(
                name: "IdStaff",
                table: "StaffTasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "IdTask",
                table: "Bills",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_StaffTasks_IdTask",
                table: "Bills",
                column: "IdTask",
                principalTable: "StaffTasks",
                principalColumn: "IdTask");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffTasks_Staffs_IdStaff",
                table: "StaffTasks",
                column: "IdStaff",
                principalTable: "Staffs",
                principalColumn: "IdStaff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_StaffTasks_IdTask",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffTasks_Staffs_IdStaff",
                table: "StaffTasks");

            migrationBuilder.AlterColumn<int>(
                name: "IdStaff",
                table: "StaffTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdTask",
                table: "Bills",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_StaffTasks_IdTask",
                table: "Bills",
                column: "IdTask",
                principalTable: "StaffTasks",
                principalColumn: "IdTask",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffTasks_Staffs_IdStaff",
                table: "StaffTasks",
                column: "IdStaff",
                principalTable: "Staffs",
                principalColumn: "IdStaff",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
