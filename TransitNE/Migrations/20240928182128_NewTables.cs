using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransitNE.Migrations
{
    /// <inheritdoc />
    public partial class NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.CreateTable(
                name: "BusTrolleyModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlockID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heading = table.Column<int>(type: "int", nullable: true),
                    Late = table.Column<bool>(type: "bit", nullable: false),
                    Original_late = table.Column<bool>(type: "bit", nullable: false),
                    Offeset_sec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Next_stop_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Next_stop_sequence = table.Column<int>(type: "int", nullable: false),
                    Estimated_seat_availability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTrolleyModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BusTrolleySchedules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stopname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCalendar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectionDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTrolleySchedules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RailScheduleModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    station = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sched_tm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    est_tm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    act_tm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RailScheduleModels", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusTrolleyModels");

            migrationBuilder.DropTable(
                name: "BusTrolleySchedules");

            migrationBuilder.DropTable(
                name: "RailScheduleModels");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
