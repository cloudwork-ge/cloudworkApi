using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudworkApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectBids",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(nullable: false),
                    projectID = table.Column<int>(nullable: false),
                    budget = table.Column<int>(nullable: false),
                    deadlineDays = table.Column<int>(nullable: false),
                    paymentCondition = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false).Annotation("Status annotation","0 - pending, 1 acceoted, -1 rejected"),
                    createDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBids", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectBids");
        }
    }
}
