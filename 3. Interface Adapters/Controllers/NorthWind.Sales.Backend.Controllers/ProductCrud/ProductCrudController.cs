namespace Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.SearchProducts;
using NorthWind.Sales.Entities.ValueObjects;

public static class ProductCrudController
{
    public static WebApplication UseProductCrudController(this WebApplication app)
    {
        app.MapPost(Endpoints.CreateProduct, CreateProduct).RequireAuthorization();
        app.MapPut(Endpoints.UpdateProduct, UpdateProduct).RequireAuthorization();
        app.MapDelete($"{Endpoints.DeleteProduct}/{{id:int}}", DeleteProduct).RequireAuthorization();
        return app;
    }

    public static async Task<IResult> CreateProduct(
        ProductDto dto,
        ICommandsRepository repository)
    {
        await repository.CreateProduct(dto);
        await repository.SaveChanges();
        return Results.Ok();
    }

    public static async Task<IResult> UpdateProduct(
        ProductDto dto,
        ICommandsRepository repository)
    {
        await repository.UpdateProduct(dto);
        await repository.SaveChanges();
        return Results.Ok();
    }

    public static async Task<IResult> DeleteProduct(
        int id,
        ICommandsRepository repository)
    {
        await repository.DeleteProduct(id);
        await repository.SaveChanges();
        return Results.Ok();
    }
}
