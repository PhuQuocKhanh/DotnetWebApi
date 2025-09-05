using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasicAuthenticationDemo.Migrations
{
    /// <inheritdoc />
    public partial class Add1 : Migration
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
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, "High performance laptop", "Laptop", 1200.00m, 15 },
                    { 2, "Ergonomic wireless mouse", "Wireless Mouse", 25.99m, 100 },
                    { 3, "RGB backlit mechanical keyboard", "Mechanical Keyboard", 79.99m, 45 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, "pranaya.rout@example.com", "Pranaya Rout", "ayRIdQoJLhXyufbY11KD1fXqmpf4aHDEXLWsdYmLZek=", "Administrator,Manager" },
                    { 2, "john.doe@example.com", "John Doe", "D03WxnvIyCeisYG8dj+auWFm2PUIQP4a4LvA53Rk2iw=", "Administrator" },
                    { 3, "jane.smith@example.com", "Jane Smith", "sKmBLz4SePwSKvTys+H4Zs9IpVJ7uaVmTJfqWPV5OzE=", "Manager" }
                });

            migrationBuilder.CreateIndex(
                name: "Index_Email_Unique",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
