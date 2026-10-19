using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ResetSalesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Start the sales workspace from a clean state while preserving user accounts
            // (they live in a separate membership database) and domain logs.
            migrationBuilder.Sql("DELETE FROM [OrderDetails]; DELETE FROM [Orders];");
            migrationBuilder.Sql("DELETE FROM [Products]; DELETE FROM [Customers];");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Name", "CurrentBalance" },
                values: new object[,]
                {
                    { "ALFKI", "Alfreds Futterkiste", 0m },
                    { "ANATR", "Ana Trujillo Emparedados y helados", 0m },
                    { "ANTON", "Antonio Moreno Taquería", 100m }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "UnitPrice", "UnitsInStock" },
                values: new object[,]
                {
                    { 1, "Chai", 35m, (short)20 },
                    { 2, "Chang", 55m, (short)0 },
                    { 3, "Aniseed Syrup", 65m, (short)20 },
                    { 4, "Chef Anton's Cajun Seasoning", 75m, (short)40 },
                    { 5, "Chef Anton's Gumbo Mix", 50m, (short)20 }
                });

            migrationBuilder.Sql("DBCC CHECKIDENT ('[Products]', RESEED, 5);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Resetting user-entered sales data is intentionally irreversible.
            // Rolling back this migration must not recreate deleted transactions.
        }
    }
}
