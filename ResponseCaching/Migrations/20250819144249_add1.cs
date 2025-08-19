using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResponseCaching.Migrations
{
    /// <inheritdoc />
    public partial class add1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product A Description", new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product A", 10.0m },
                    { 2, new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product B Description", new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product B", 20.0m },
                    { 3, new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product C Description", new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product C", 30.0m },
                    { 4, new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product D Description", new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product D", 40.0m },
                    { 5, new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product E Description", new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Product E", 50.0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
