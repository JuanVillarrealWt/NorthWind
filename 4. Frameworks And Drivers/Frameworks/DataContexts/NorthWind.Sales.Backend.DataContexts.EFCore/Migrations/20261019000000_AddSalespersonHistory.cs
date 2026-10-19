using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

#nullable disable
namespace NorthWind.Sales.Backend.DataContexts.EFCore.Migrations;

[DbContext(typeof(NorthWindContext))]
[Migration("20261019000000_AddSalespersonHistory")]
public partial class AddSalespersonHistory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "SalespersonName",
            table: "Orders",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "Vendedor no registrado");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SalespersonName",
            table: "Orders");
    }
}
