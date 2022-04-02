using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UIM.Core.Data.Migrations
{
    public partial class Attachment_AddProps_NameFieldId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FileId", table: "Attachments");

            migrationBuilder.DropColumn(name: "Name", table: "Attachments");
        }
    }
}
