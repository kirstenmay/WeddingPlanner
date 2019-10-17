using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingPlanner.Migrations
{
    public partial class ChangedCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_CreatorUserId",
                table: "Weddings");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_CreatorUserId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Weddings");

            migrationBuilder.AlterColumn<string>(
                name: "WedderTwo",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WedderOne",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Creator",
                table: "Weddings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Weddings");

            migrationBuilder.AlterColumn<string>(
                name: "WedderTwo",
                table: "Weddings",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "WedderOne",
                table: "Weddings",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Weddings",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "Weddings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_CreatorUserId",
                table: "Weddings",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_CreatorUserId",
                table: "Weddings",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
