using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace NorthWind.Sales.Backend.DataContexts.EFCore.Migrations;
public partial class AddProductAndCustomer : Migration
{
 protected override void Up(MigrationBuilder migrationBuilder)
 {
  migrationBuilder.CreateTable("Customers", table => new
  {
   Id=table.Column<string>(type:"nchar(5)",fixedLength:true,maxLength:5,nullable:false),
   Name=table.Column<string>(type:"nvarchar(40)",maxLength:40,nullable:false),
   CurrentBalance=table.Column<decimal>(type:"decimal(8,2)",precision:8,scale:2,nullable:false)
  }, constraints: table=>table.PrimaryKey("PK_Customers",x=>x.Id));
  migrationBuilder.CreateTable("Products", table => new
  {
   Id=table.Column<int>(type:"int",nullable:false).Annotation("SqlServer:Identity","1, 1"),
   Name=table.Column<string>(type:"nvarchar(40)",maxLength:40,nullable:false),
   UnitsInStock=table.Column<short>(type:"smallint",nullable:false),
   UnitPrice=table.Column<decimal>(type:"decimal(8,2)",precision:8,scale:2,nullable:false)
  }, constraints: table=>table.PrimaryKey("PK_Products",x=>x.Id));
  migrationBuilder.InsertData("Customers", new[]{"Id","Name","CurrentBalance"}, new object[,] {
   {"ALFKI","Alfreds Futterkiste",0m},{"ANATR","Ana Trujillo Emparedados y helados",0m},{"ANTON","Antonio Moreno Taquería",100m}});
  migrationBuilder.InsertData("Products", new[]{"Id","Name","UnitPrice","UnitsInStock"}, new object[,] {
   {1,"Chai",35m,(short)20},{2,"Chang",55m,(short)0},{3,"Aniseed Syrup",65m,(short)20},{4,"Chef Anton's Cajun Seasoning",75m,(short)40},{5,"Chef Anton's Gumbo Mix",50m,(short)20}});
  migrationBuilder.CreateIndex("IX_Orders_CustomerId","Orders","CustomerId");
  migrationBuilder.CreateIndex("IX_OrderDetails_ProductId","OrderDetails","ProductId");
  migrationBuilder.AddForeignKey("FK_Orders_Customers_CustomerId","Orders","CustomerId","Customers","Id",onDelete:ReferentialAction.Cascade);
  migrationBuilder.AddForeignKey("FK_OrderDetails_Products_ProductId","OrderDetails","ProductId","Products","Id",onDelete:ReferentialAction.Cascade);
 }
 protected override void Down(MigrationBuilder migrationBuilder)
 {
  migrationBuilder.DropForeignKey("FK_Orders_Customers_CustomerId","Orders");
  migrationBuilder.DropForeignKey("FK_OrderDetails_Products_ProductId","OrderDetails");
  migrationBuilder.DropIndex("IX_Orders_CustomerId","Orders");
  migrationBuilder.DropIndex("IX_OrderDetails_ProductId","OrderDetails");
  migrationBuilder.DropTable("Customers"); migrationBuilder.DropTable("Products");
 }
}
