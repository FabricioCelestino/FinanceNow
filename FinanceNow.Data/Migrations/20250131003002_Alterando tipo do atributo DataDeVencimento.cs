using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceNow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterandotipodoatributoDataDeVencimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataDeVencimento",
                table: "Transacoes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataDeVencimento",
                table: "Transacoes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
