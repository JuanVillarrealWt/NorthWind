namespace Microsoft.AspNetCore.Builder;

internal static class UserLoginController
{
    public static WebApplication UseUserLoginController(this WebApplication app)
    {
        app.MapPost(NorthWind.Membership.Entities.ValueObjects.Endpoints.Login,
            async (NorthWind.Membership.Entities.Dtos.UserLogin.UserCredentialsDto userCredentials,
                NorthWind.Membership.Backend.Core.Interfaces.UserLogin.IUserLoginInputPort inputPort,
                NorthWind.Membership.Backend.Core.Interfaces.UserLogin.IUserLoginOutputPort presenter) =>
            {
                await inputPort.Handle(userCredentials);
                return presenter.Result;
            });
        return app;
    }
}
