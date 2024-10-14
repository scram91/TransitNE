using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransitNE.Migrations
{
    /// <inheritdoc />
    public partial class BusStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusTrolleyRouteModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTrolleyRouteModels", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusTrolleyRouteModels");
        }
    }
}
