using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWarrantlyRecordGrpcServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "WarrantyRecords",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "WarrantyRecords",
                newName: "status");
        }
    }
}
