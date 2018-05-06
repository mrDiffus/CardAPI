using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CardAPI.Migrations
{
    public partial class InitalCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardName = table.Column<string>(nullable: true),
                    CardNumber = table.Column<int>(nullable: false),
                    CardType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardID);
                });

            migrationBuilder.CreateTable(
                name: "CardOptions",
                columns: table => new
                {
                    CardOptionID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(nullable: true),
                    CardID = table.Column<int>(nullable: true),
                    Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardOptions", x => x.CardOptionID);
                    table.ForeignKey(
                        name: "FK_CardOptions_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "CardID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false),
                    CardOptionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => new { x.CardID, x.CardOptionID });
                    table.ForeignKey(
                        name: "FK_Stage_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "CardID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stage_CardOptions_CardOptionID",
                        column: x => x.CardOptionID,
                        principalTable: "CardOptions",
                        principalColumn: "CardOptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardOptions_CardID",
                table: "CardOptions",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_CardOptionID",
                table: "Stage",
                column: "CardOptionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.DropTable(
                name: "CardOptions");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
