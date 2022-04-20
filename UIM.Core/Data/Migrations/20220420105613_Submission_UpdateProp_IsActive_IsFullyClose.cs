using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UIM.Core.Data.Migrations
{
    public partial class Submission_UpdateProp_IsActive_IsFullyClose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Submissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Submissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
