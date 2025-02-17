using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkiServerBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAuftraege_Mitarbeiter_MitarbeiterId",
                table: "ServiceAuftraege");

            migrationBuilder.DropIndex(
                name: "IX_ServiceAuftraege_MitarbeiterId",
                table: "ServiceAuftraege");

            migrationBuilder.DropColumn(
                name: "Beschreibung",
                table: "ServiceAuftraege");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ServiceAuftraege");

            migrationBuilder.RenameColumn(
                name: "MitarbeiterId",
                table: "ServiceAuftraege",
                newName: "MitarbeiterID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ServiceAuftraege",
                newName: "AuftragID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Mitarbeiter",
                newName: "MitarbeiterID");

            migrationBuilder.AlterColumn<int>(
                name: "Priorität",
                table: "ServiceAuftraege",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DienstleistungID",
                table: "ServiceAuftraege",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Erstellungsdatum",
                table: "ServiceAuftraege",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KundeId",
                table: "ServiceAuftraege",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "ServiceAuftraege",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Dienstleistung",
                columns: table => new
                {
                    DienstleistungId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preis = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dienstleistung", x => x.DienstleistungId);
                });

            migrationBuilder.CreateTable(
                name: "Kunden",
                columns: table => new
                {
                    KundeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunden", x => x.KundeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAuftraege_DienstleistungID",
                table: "ServiceAuftraege",
                column: "DienstleistungID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAuftraege_KundeId",
                table: "ServiceAuftraege",
                column: "KundeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAuftraege_Dienstleistung_DienstleistungID",
                table: "ServiceAuftraege",
                column: "DienstleistungID",
                principalTable: "Dienstleistung",
                principalColumn: "DienstleistungId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAuftraege_Kunden_KundeId",
                table: "ServiceAuftraege",
                column: "KundeId",
                principalTable: "Kunden",
                principalColumn: "KundeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAuftraege_Dienstleistung_DienstleistungID",
                table: "ServiceAuftraege");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAuftraege_Kunden_KundeId",
                table: "ServiceAuftraege");

            migrationBuilder.DropTable(
                name: "Dienstleistung");

            migrationBuilder.DropTable(
                name: "Kunden");

            migrationBuilder.DropIndex(
                name: "IX_ServiceAuftraege_DienstleistungID",
                table: "ServiceAuftraege");

            migrationBuilder.DropIndex(
                name: "IX_ServiceAuftraege_KundeId",
                table: "ServiceAuftraege");

            migrationBuilder.DropColumn(
                name: "DienstleistungID",
                table: "ServiceAuftraege");

            migrationBuilder.DropColumn(
                name: "Erstellungsdatum",
                table: "ServiceAuftraege");

            migrationBuilder.DropColumn(
                name: "KundeId",
                table: "ServiceAuftraege");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "ServiceAuftraege");

            migrationBuilder.RenameColumn(
                name: "MitarbeiterID",
                table: "ServiceAuftraege",
                newName: "MitarbeiterId");

            migrationBuilder.RenameColumn(
                name: "AuftragID",
                table: "ServiceAuftraege",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MitarbeiterID",
                table: "Mitarbeiter",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Priorität",
                table: "ServiceAuftraege",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Beschreibung",
                table: "ServiceAuftraege",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ServiceAuftraege",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAuftraege_MitarbeiterId",
                table: "ServiceAuftraege",
                column: "MitarbeiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAuftraege_Mitarbeiter_MitarbeiterId",
                table: "ServiceAuftraege",
                column: "MitarbeiterId",
                principalTable: "Mitarbeiter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
