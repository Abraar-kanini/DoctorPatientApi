using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorPatient.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "patients");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "patients");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "patients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
