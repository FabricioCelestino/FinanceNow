using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceNow.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExcluindoatabelaMes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacoes_Meses_MedId",
                table: "Transacoes");

            migrationBuilder.DropTable(
                name: "Meses");

            migrationBuilder.DropIndex(
                name: "IX_Transacoes_MedId",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "MedId",
                table: "Transacoes");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedId",
                table: "Transacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "Categorias",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Meses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    MesNumero = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_MedId",
                table: "Transacoes",
                column: "MedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacoes_Meses_MedId",
                table: "Transacoes",
                column: "MedId",
                principalTable: "Meses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
