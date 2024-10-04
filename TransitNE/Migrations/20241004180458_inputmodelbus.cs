using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransitNE.Migrations
{
    /// <inheritdoc />
    public partial class inputmodelbus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusInputModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Busline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StopName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusInputModels", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusInputModels");
        }
    }
}
