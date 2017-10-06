using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartPlayerAPI.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateOfCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Club", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    NameOfGame = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOfStart = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeighOfUser = table.Column<int>(type: "int", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeightOfUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerInGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerInGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerInGame_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerInGame_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccelerometerAndGyroscopeResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerInGameId = table.Column<int>(type: "int", nullable: false),
                    TimeOfOccur = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccelerometerAndGyroscopeResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccelerometerAndGyroscopeResult_PlayerInGame_PlayerInGameId",
                        column: x => x.PlayerInGameId,
                        principalTable: "PlayerInGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PulseSensorResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerInGameId = table.Column<int>(type: "int", nullable: false),
                    TimeOfOccur = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PulseSensorResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PulseSensorResult_PlayerInGame_PlayerInGameId",
                        column: x => x.PlayerInGameId,
                        principalTable: "PlayerInGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccelerometerAndGyroscopeResult_PlayerInGameId",
                table: "AccelerometerAndGyroscopeResult",
                column: "PlayerInGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ClubId",
                table: "Game",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ClubId",
                table: "Player",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerInGame_GameId",
                table: "PlayerInGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerInGame_PlayerId",
                table: "PlayerInGame",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PulseSensorResult_PlayerInGameId",
                table: "PulseSensorResult",
                column: "PlayerInGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccelerometerAndGyroscopeResult");

            migrationBuilder.DropTable(
                name: "PulseSensorResult");

            migrationBuilder.DropTable(
                name: "PlayerInGame");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Club");
        }
    }
}
