using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Database.Migrations
{
    public partial class DateTimeFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SentOn",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 1, 1, 1, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SentOn",
                table: "Notifications",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(1, 1, 1, 1, 1, 1, 0, DateTimeKind.Unspecified));
        }
    }
}
