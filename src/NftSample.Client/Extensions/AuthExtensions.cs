using NftSample.Client.Infraestructure;
using Microsoft.AspNetCore.Components.Authorization;
using Nethereum.Blazor;
using Nethereum.Metamask;
using Nethereum.Metamask.Blazor;
using Nethereum.Siwe;
using Nethereum.UI;

namespace NftSample.Client.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddSingleton<UserState>();
        services.AddSingleton<IMetamaskInterop, MetamaskBlazorInterop>();
        services.AddSingleton<IEthereumHostProvider, MetamaskHostProvider>();

//Add metamask as the selected ethereum host provider
        services.AddSingleton(services =>
        {
            var metamaskHostProvider = services.GetService<IEthereumHostProvider>();


            var selectedHostProvider = new SelectedEthereumHostProviderService();
            selectedHostProvider.SetSelectedEthereumHostProvider(metamaskHostProvider);
            return selectedHostProvider;
        });

        services.AddSingleton<AuthenticationStateProvider, EthereumAuthenticationStateProvider>();


        var inMemorySessionNonceStorage = new InMemorySessionNonceStorage();
        services.AddSingleton<ISessionStorage>(x => inMemorySessionNonceStorage);

        services.AddSingleton<NethereumSiweAuthenticatorService>();
        services.AddSingleton<IAccessTokenService, LocalStorageAccessTokenService>();
        services.AddSingleton<SiweApiUserLoginService>();
        services.AddSingleton<SiweAuthenticationStateProvider>();
        services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetService<SiweAuthenticationStateProvider>());

        services.AddAuthorizationCore();
        return services;
    }
}