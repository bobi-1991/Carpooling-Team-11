using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPooling.Data.Migrations
{
    public partial class seats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableSlots",
                table: "Travels",
                newName: "AvailableSeats");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "Travels",
                newName: "AvailableSlots");
        }
    }
}
