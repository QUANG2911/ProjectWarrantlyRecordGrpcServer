using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWarrantlyRecordGrpcServer.Migrations
{
    /// <inheritdoc />
    public partial class AlterAddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReasonBringFix",
                table: "StaffTasks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonBringFix",
                table: "StaffTasks");
        }
    }
}
