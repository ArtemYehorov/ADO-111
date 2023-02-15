using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Migrations
{
    /// <inheritdoc />
    public partial class Sale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdManager = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProduct = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Moment = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "Count", "IdManager", "IdProduct", "Moment" },
                values: new object[] { new Guid("72098e8b-1298-40bc-a612-0007e5aede94"), 6, new Guid("23d52416-d994-4564-a106-1fdf5fecef25"), new Guid("870da1a9-44f4-4018-b7fc-727a2058faf0"), new DateTime(2022, 6, 5, 11, 5, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
