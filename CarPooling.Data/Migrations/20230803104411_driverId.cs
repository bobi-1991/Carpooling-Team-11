using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPooling.Data.Migrations
{
    public partial class driverId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_AspNetUsers_UserId",
                table: "TripRequests");

            migrationBuilder.DropIndex(
                name: "IX_TripRequests_UserId",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TripRequests");

            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "TripRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_DriverId",
                table: "TripRequests",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_AspNetUsers_DriverId",
                table: "TripRequests",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_AspNetUsers_DriverId",
                table: "TripRequests");

            migrationBuilder.DropIndex(
                name: "IX_TripRequests_DriverId",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "TripRequests");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TripRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_UserId",
                table: "TripRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_AspNetUsers_UserId",
                table: "TripRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
