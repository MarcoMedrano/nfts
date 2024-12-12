using NftSample.Infraestructure.Api;

namespace NftSample.Client.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddApiService(this IServiceCollection services, string baseUrl)
    {
        services.AddScoped<BearerTokenHandler>();

        services.AddRefit<IUsersController>(baseUrl);
        services.AddRefit<INftsController>(baseUrl);

        return services;
    }
}