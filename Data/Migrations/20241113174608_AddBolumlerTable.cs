using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DersPrgProjesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBolumlerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bolumler",
                columns: table => new
                {
                    BolumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BolumAd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BolumMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BolumSifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FakulteID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolumler", x => x.BolumID);
                    table.ForeignKey(
                        name: "FK_Bolumler_Fakulteler_FakulteID",
                        column: x => x.FakulteID,
                        principalTable: "Fakulteler",
                        principalColumn: "FakulteID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bolumler_FakulteID",
                table: "Bolumler",
                column: "FakulteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bolumler");
        }
    }
}
