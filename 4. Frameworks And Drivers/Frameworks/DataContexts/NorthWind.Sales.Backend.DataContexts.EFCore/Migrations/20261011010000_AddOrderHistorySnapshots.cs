using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

#nullable disable
namespace NorthWind.Sales.Backend.DataContexts.EFCore.Migrations;

[DbContext(typeof(NorthWindContext))]
[Migration("20261011010000_AddOrderHistorySnapshots")]
public partial class AddOrderHistorySnapshots : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders");
        migrationBuilder.DropForeignKey(
            name: "FK_OrderDetails_Products_ProductId",
            table: "OrderDetails");

        migrationBuilder.AddColumn<string>(
            name: "CustomerName",
            table: "Orders",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: true);
        migrationBuilder.AddColumn<string>(
            name: "ProductName",
            table: "OrderDetails",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: true);

        migrationBuilder.Sql("""
            UPDATE [Orders]
            SET [CustomerName] = COALESCE([Customers].[Name], [Orders].[CustomerId])
            FROM [Orders]
            LEFT JOIN [Customers] ON [Customers].[Id] = [Orders].[CustomerId];

            UPDATE [OrderDetails]
            SET [ProductName] = COALESCE([Products].[Name], CONCAT(N'Producto #', [OrderDetails].[ProductId]))
            FROM [OrderDetails]
            LEFT JOIN [Products] ON [Products].[Id] = [OrderDetails].[ProductId];
            """);

        migrationBuilder.AlterColumn<string>(
            name: "CustomerName",
            table: "Orders",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(40)",
            oldMaxLength: 40,
            oldNullable: true);
        migrationBuilder.AlterColumn<string>(
            name: "ProductName",
            table: "OrderDetails",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(40)",
            oldMaxLength: 40,
            oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "CustomerName", table: "Orders");
        migrationBuilder.DropColumn(name: "ProductName", table: "OrderDetails");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders",
            column: "CustomerId",
            principalTable: "Customers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        migrationBuilder.AddForeignKey(
            name: "FK_OrderDetails_Products_ProductId",
            table: "OrderDetails",
            column: "ProductId",
            principalTable: "Products",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
