using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Og.Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NativeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CultureName = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    UniqueSeoCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    FlagImageFileName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Rtl = table.Column<bool>(type: "bit", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbLanguages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbLanguages_UniqueSeoCode",
                table: "TbLanguages",
                column: "UniqueSeoCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbLanguages");
        }
    }
}
