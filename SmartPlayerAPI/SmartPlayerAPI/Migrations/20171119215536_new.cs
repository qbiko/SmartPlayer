using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartPlayerAPI.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_SmartPlayerAPI.Persistance.Models.Pitch_PitchId",
                table: "Game");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmartPlayerAPI.Persistance.Models.Pitch_TempId",
                table: "SmartPlayerAPI.Persistance.Models.Pitch");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "SmartPlayerAPI.Persistance.Models.Pitch");

            migrationBuilder.RenameTable(
                name: "SmartPlayerAPI.Persistance.Models.Pitch",
                newName: "Pitch");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Pitch",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeftDownPoint",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeftUpPoint",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameOfPitch",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightDownPoint",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightUpPoint",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Pitch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pitch",
                table: "Pitch",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pitch",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "LeftDownPoint",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "LeftUpPoint",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "NameOfPitch",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "RightDownPoint",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "RightUpPoint",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Pitch");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Pitch");

            migrationBuilder.RenameTable(
                name: "Pitch",
                newName: "SmartPlayerAPI.Persistance.Models.Pitch");

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "SmartPlayerAPI.Persistance.Models.Pitch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmartPlayerAPI.Persistance.Models.Pitch_TempId",
                table: "SmartPlayerAPI.Persistance.Models.Pitch",
                column: "TempId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_SmartPlayerAPI.Persistance.Models.Pitch_PitchId",
                table: "Game",
                column: "PitchId",
                principalTable: "SmartPlayerAPI.Persistance.Models.Pitch",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
