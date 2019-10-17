using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingPlanner.Migrations
{
    public partial class ChangeWeddingClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_WedderOneId",
                table: "Weddings");

            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_WedderTwoId",
                table: "Weddings");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_WedderOneId",
                table: "Weddings");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_WedderTwoId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "WedderOneId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "WedderTwoId",
                table: "Weddings");

            migrationBuilder.AddColumn<string>(
                name: "WedderOne",
                table: "Weddings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WedderTwo",
                table: "Weddings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LoginEmail = table.Column<string>(nullable: false),
                    LoginPassword = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginUsers", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginUsers");

            migrationBuilder.DropColumn(
                name: "WedderOne",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "WedderTwo",
                table: "Weddings");

            migrationBuilder.AddColumn<int>(
                name: "WedderOneId",
                table: "Weddings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WedderTwoId",
                table: "Weddings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_WedderOneId",
                table: "Weddings",
                column: "WedderOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_WedderTwoId",
                table: "Weddings",
                column: "WedderTwoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_WedderOneId",
                table: "Weddings",
                column: "WedderOneId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_WedderTwoId",
                table: "Weddings",
                column: "WedderTwoId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
