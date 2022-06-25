using Microsoft.EntityFrameworkCore.Migrations;

namespace MasMasr.Migrations
{
    public partial class AddingNewFieldsToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPhone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File3",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TermsAndConditions",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserApproved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyPhone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "File1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "File2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "File3",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobTitile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TermsAndConditions",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserApproved",
                table: "AspNetUsers");
        }
    }
}
