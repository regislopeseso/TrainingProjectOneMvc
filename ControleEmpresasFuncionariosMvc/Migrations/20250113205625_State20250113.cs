using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleEmpresasFuncionariosMvc.Migrations
{
    /// <inheritdoc />
    public partial class State20250113 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobsQty",
                table: "Company");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobsQty",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
