using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EVoucherGroupId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "EventCategoryName",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "EventLocation",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "EventName",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "EventTime",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Tickets",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "Tickets",
                newName: "EventId");

            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "Tickets",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tickets",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Tickets",
                newName: "InvoiceId");

            migrationBuilder.AddColumn<string>(
                name: "EVoucherGroupId",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventCategoryName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EventDate",
                table: "Tickets",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "EventLocation",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EventTime",
                table: "Tickets",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
