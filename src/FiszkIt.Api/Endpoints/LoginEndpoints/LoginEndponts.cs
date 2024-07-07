namespace FiszkIt.Api.Endpoints.LoginEndpoints;

public static class LoginEndpoints
{
    public static IEndpointRouteBuilder RegisterLoginEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetLogin()
            .MapGetToken();

        return app;
    }
}