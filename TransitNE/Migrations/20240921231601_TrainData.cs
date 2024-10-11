using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransitNE.Migrations
{
    /// <inheritdoc />
    public partial class TrainData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainModel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trainno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    service = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentstop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nextstop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    line = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    consist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    heading = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    late = table.Column<int>(type: "int", nullable: true),
                    SOURCE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRACK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRACK_CHANGE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainModel", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainModel");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
