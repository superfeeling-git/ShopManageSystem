using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Entity.Migrations
{
    public partial class _202134 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "SmsUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "SmsUser");
        }
    }
}
