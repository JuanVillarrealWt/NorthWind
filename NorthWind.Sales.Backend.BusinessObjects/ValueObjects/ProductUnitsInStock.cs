namespace NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
public sealed class ProductUnitsInStock(int productId, short unitsInStock)
{ public int ProductId { get; } = productId; public short UnitsInStock { get; } = unitsInStock; }
