using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AESServerAPP.Migrations
{
    /// <inheritdoc />
    public partial class Add1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientKeyIVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IV = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientKeyIVs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ClientKeyIVs",
                columns: new[] { "Id", "ClientId", "IV", "Key" },
                values: new object[] { 1, "DefaultClient", "/X9EAc4vBALd31ye7N3L1g==", "Yyj9nVLtBLwPANTqZNFHrofcH/AbvJlaUbytoHT8Qd8=" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Name", "Salary" },
                values: new object[] { 1, "Alice Smith", 75000m });

            migrationBuilder.CreateIndex(
                name: "IX_ClientKeyIVs_ClientId",
                table: "ClientKeyIVs",
                column: "ClientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientKeyIVs");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
