namespace Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.SearchCustomers;
using NorthWind.Sales.Entities.ValueObjects;

public static class CustomerCrudController
{
    public static WebApplication UseCustomerCrudController(this WebApplication app)
    {
        app.MapPost(Endpoints.CreateCustomer, CreateCustomer).RequireAuthorization();
        app.MapPut(Endpoints.UpdateCustomer, UpdateCustomer).RequireAuthorization();
        app.MapDelete($"{Endpoints.DeleteCustomer}/{{id}}", DeleteCustomer).RequireAuthorization();
        return app;
    }

    public static async Task<IResult> CreateCustomer(
        CustomerDto dto,
        ICommandsRepository repository)
    {
        await repository.CreateCustomer(dto);
        await repository.SaveChanges();
        return Results.Ok();
    }

    public static async Task<IResult> UpdateCustomer(
        CustomerDto dto,
        ICommandsRepository repository)
    {
        await repository.UpdateCustomer(dto);
        await repository.SaveChanges();
        return Results.Ok();
    }

    public static async Task<IResult> DeleteCustomer(
        string id,
        ICommandsRepository repository)
    {
        await repository.DeleteCustomer(id);
        await repository.SaveChanges();
        return Results.Ok();
    }
}
