using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceNow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarcampoRecorrenteatabelatransacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Recorrente",
                table: "Transacoes",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recorrente",
                table: "Transacoes");
        }
    }
}
