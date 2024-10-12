namespace FiszkIt.Api.Endpoints.AuthEndpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder RegisterAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateUser();
        app.MapConfirmUser();
        app.MapLoginUser();
        return app;
    }
}