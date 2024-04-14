using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPageService.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Warranty = table.Column<string>(type: "text", nullable: false),
                    Speed = table.Column<string>(type: "text", nullable: false),
                    Power = table.Column<string>(type: "text", nullable: false),
                    Acceleration = table.Column<string>(type: "text", nullable: false),
                    FuelConsumption = table.Column<string>(type: "text", nullable: false),
                    Package = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarPages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarPages");
        }
    }
}
