using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Entity.Migrations
{
    public partial class _202104151 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "SmsGoods",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "SmsGoods");
        }
    }
}
