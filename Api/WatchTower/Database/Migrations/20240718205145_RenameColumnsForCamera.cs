using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchTower.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnsForCamera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Cameras");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cameras",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Cameras",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Cameras");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cameras",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Cameras",
                type: "text",
                nullable: true);
        }
    }
}
