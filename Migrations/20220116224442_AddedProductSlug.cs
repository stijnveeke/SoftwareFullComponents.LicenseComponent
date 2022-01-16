using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareFullComponents.LicenseComponent.Migrations
{
    public partial class AddedProductSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductSlug",
                table: "License",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSlug",
                table: "License");
        }
    }
}
