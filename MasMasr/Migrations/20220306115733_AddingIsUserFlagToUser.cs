using Microsoft.EntityFrameworkCore.Migrations;

namespace MasMasr.Migrations
{
    public partial class AddingIsUserFlagToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUser",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUser",
                table: "AspNetUsers");
        }
    }
}
