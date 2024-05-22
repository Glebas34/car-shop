using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace imageservice.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCarPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    CarPageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Root = table.Column<string>(type: "text", nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarPages");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
