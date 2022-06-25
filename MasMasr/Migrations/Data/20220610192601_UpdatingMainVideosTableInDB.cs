using Microsoft.EntityFrameworkCore.Migrations;

namespace MasMasr.Migrations.Data
{
    public partial class UpdatingMainVideosTableInDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalLink",
                table: "MainVideos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalLink",
                table: "MainVideos");
        }
    }
}
