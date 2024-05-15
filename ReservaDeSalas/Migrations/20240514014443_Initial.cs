using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaDeSalas.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Usuario = table.Column<string>(type: "TEXT", nullable: false),
                    Sala = table.Column<string>(type: "TEXT", nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservas");
        }
    }
}
