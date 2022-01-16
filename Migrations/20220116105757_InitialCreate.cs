using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareFullComponents.LicenseComponent.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "License",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseKey = table.Column<string>(type: "varchar(100)", nullable: true),
                    UserIdentifier = table.Column<string>(type: "varchar(255)", nullable: true),
                    TimesActivated = table.Column<int>(type: "int", nullable: false),
                    ActivateableAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "License");
        }
    }
}
