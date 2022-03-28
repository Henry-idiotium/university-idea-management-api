using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UIM.Core.Data.Migrations
{
    public partial class AppUser_AddProp_IsDefaultPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultPassword",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultPassword",
                table: "AspNetUsers");
        }
    }
}
