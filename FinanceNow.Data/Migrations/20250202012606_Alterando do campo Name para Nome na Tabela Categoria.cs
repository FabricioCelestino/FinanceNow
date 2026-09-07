using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceNow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterandodocampoNameparaNomenaTabelaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categorias",
                newName: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Categorias",
                newName: "Name");
        }
    }
}
