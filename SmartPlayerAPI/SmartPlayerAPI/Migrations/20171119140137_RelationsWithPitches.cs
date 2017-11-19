using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartPlayerAPI.Migrations
{
    public partial class RelationsWithPitches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PitchId",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_PitchId",
                table: "Game",
                column: "PitchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Pitch_PitchId",
                table: "Game",
                column: "PitchId",
                principalTable: "Pitch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Pitch_PitchId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_PitchId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "PitchId",
                table: "Game");
        }
    }
}
