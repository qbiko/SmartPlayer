using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartPlayerAPI.Migrations
{
    public partial class latlng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "AccelerometerAndGyroscopeResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                table: "AccelerometerAndGyroscopeResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "AccelerometerAndGyroscopeResult");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "AccelerometerAndGyroscopeResult");

        }
    }
}
