using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyShop.Data.Migrations
{
    public partial class CreateTableUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UnionId = table.Column<string>(nullable: true),
                    OpenId = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    HeadImg = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
