using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication1.Migrations
{
    public partial class FixAttempt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId1",
                table: "Attempts");

            migrationBuilder.DropIndex(
                name: "IX_Attempts_UserId1",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Attempts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Attempts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Attempts_UserId",
                table: "Attempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId",
                table: "Attempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId",
                table: "Attempts");

            migrationBuilder.DropIndex(
                name: "IX_Attempts_UserId",
                table: "Attempts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Attempts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Attempts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attempts_UserId1",
                table: "Attempts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId1",
                table: "Attempts",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
