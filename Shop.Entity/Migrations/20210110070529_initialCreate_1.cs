using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Entity.Migrations
{
    public partial class initialCreate_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    ParentPath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "SmsRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsGoods",
                columns: table => new
                {
                    GoodsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GoodsPic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GoodsPrice = table.Column<decimal>(type: "money", nullable: false),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsGoods", x => x.GoodsId);
                    table.ForeignKey(
                        name: "FK_SmsGoods_SmsCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SmsCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsRoleClaim_SmsRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SmsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsUserClaim_SmsUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SmsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_SmsUserLogin_SmsUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SmsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsUserRole",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_SmsUserRole_SmsRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SmsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsUserRole_SmsUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SmsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsUserToken",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_SmsUserToken_SmsUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SmsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SmsCategory",
                columns: new[] { "CategoryId", "CategoryName", "Depth", "ParentId", "ParentPath" },
                values: new object[] { 1, "家用电器", 0, 0, "0" });

            migrationBuilder.InsertData(
                table: "SmsCategory",
                columns: new[] { "CategoryId", "CategoryName", "Depth", "ParentId", "ParentPath" },
                values: new object[] { 2, "床上服务器", 0, 0, "0" });

            migrationBuilder.CreateIndex(
                name: "IX_SmsGoods_CategoryId",
                table: "SmsGoods",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "SmsRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SmsRoleClaim_RoleId",
                table: "SmsRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "SmsUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "SmsUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SmsUserClaim_UserId",
                table: "SmsUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsUserLogin_UserId",
                table: "SmsUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsUserRole_RoleId",
                table: "SmsUserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsGoods");

            migrationBuilder.DropTable(
                name: "SmsRoleClaim");

            migrationBuilder.DropTable(
                name: "SmsUserClaim");

            migrationBuilder.DropTable(
                name: "SmsUserLogin");

            migrationBuilder.DropTable(
                name: "SmsUserRole");

            migrationBuilder.DropTable(
                name: "SmsUserToken");

            migrationBuilder.DropTable(
                name: "SmsCategory");

            migrationBuilder.DropTable(
                name: "SmsRole");

            migrationBuilder.DropTable(
                name: "SmsUser");
        }
    }
}
