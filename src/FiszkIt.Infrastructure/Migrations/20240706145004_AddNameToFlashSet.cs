using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiszkIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToFlashSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FlashSets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FlashSets");
        }
    }
}
