using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartPlayerAPI.Migrations
{
    public partial class sdfds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Module",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Module_ClubId",
                table: "Module",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Club_ClubId",
                table: "Module",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Club_ClubId",
                table: "Module");

            migrationBuilder.DropIndex(
                name: "IX_Module_ClubId",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Module");
        }
    }
}
