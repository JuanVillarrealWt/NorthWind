namespace Microsoft.AspNetCore.Builder;

internal static class UserRegistrationController
{
    public static WebApplication UseUserRegistrationController(this WebApplication app)
    {
        app.MapPost(NorthWind.Membership.Entities.ValueObjects.Endpoints.Register,
            async (NorthWind.Membership.Entities.Dtos.UserRegistration.UserRegistrationDto userData,
                NorthWind.Membership.Backend.Core.Interfaces.UserRegistration.IUserRegistrationInputPort inputPort,
                NorthWind.Membership.Backend.Core.Interfaces.UserRegistration.IUserRegistrationOutputPort presenter) =>
            {
                await inputPort.Handle(userData);
                return presenter.Result;
            });
        return app;
    }
}
