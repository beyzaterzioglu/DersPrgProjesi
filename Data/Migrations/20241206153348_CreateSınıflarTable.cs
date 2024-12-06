using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DersPrgProjesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSınıflarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sınıflar",
                columns: table => new
                {
                    SınıfID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SınıfAd = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kapasite = table.Column<int>(type: "int", nullable: false),
                    SınavKapasite = table.Column<int>(type: "int", nullable: false),
                    FakulteID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sınıflar", x => x.SınıfID);
                    table.ForeignKey(
                        name: "FK_Sınıflar_Fakulteler_FakulteID",
                        column: x => x.FakulteID,
                        principalTable: "Fakulteler",
                        principalColumn: "FakulteID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sınıflar_FakulteID",
                table: "Sınıflar",
                column: "FakulteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sınıflar");
        }
    }
}
