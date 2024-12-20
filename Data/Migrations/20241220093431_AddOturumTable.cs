using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DersPrgProjesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOturumTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Oturumlar",
                columns: table => new
                {
                    OturumId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    Gun = table.Column<int>(type: "int", nullable: false),
                    SınıfID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oturumlar", x => x.OturumId);
                    table.ForeignKey(
                        name: "FK_Oturumlar_Sınıflar_SınıfID",
                        column: x => x.SınıfID,
                        principalTable: "Sınıflar",
                        principalColumn: "SınıfID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oturumlar_SınıfID",
                table: "Oturumlar",
                column: "SınıfID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Oturumlar");
        }
    }
}
