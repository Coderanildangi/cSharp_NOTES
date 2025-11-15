using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Note_Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "CreatedAt", "Tag", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { "a2a9d8c3-4e6f-4c5b-9d0a-1b3c4f5e6d7a", "Initial meeting notes for the new Q4 feature.", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "idea", "Project Alpha", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { "b9b8c7a6-1d2e-3f4g-5h6i-7j8k9l0m1n2o", "My secret plan to take over the world. Only access with PIN.", new DateTime(2025, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc), "confidential", "Hush Hush Details", new DateTime(2025, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { "c1c2d3e4-5f6g-7h8i-9j0k-1l2m3n4o5p6q", "Milk, Eggs, Bread.", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "todo", "Grocery List", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");
        }
    }
}
