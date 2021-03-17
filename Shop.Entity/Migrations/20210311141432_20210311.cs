using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Entity.Migrations
{
    public partial class _20210311 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsSysMenu",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    ParentPath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsSysMenu", x => x.MenuId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsSysMenu");
        }
    }
}
