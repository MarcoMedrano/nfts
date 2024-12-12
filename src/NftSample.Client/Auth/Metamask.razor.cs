
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Nethereum.Blazor;
using Nethereum.UI;

namespace NftSample.Client.Auth;

public partial class Metamask
{
    private bool authorizingInProgress;
    private UserDto User => UserState!.User;
    [CascadingParameter] private UserState UserState { get; set; } = null!;

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private IEthereumHostProvider EthHostProvider { get; set; } = null!;
    [Inject] SiweAuthenticationStateProvider AuthState { get; set; }


    private bool MetamaskAvailable { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        MetamaskAvailable = await EthHostProvider.CheckProviderAvailabilityAsync();
        StateHasChanged();
    }

    protected async Task LoginWithMetaMask()
    {
        authorizingInProgress = true;
        try
        {
            await EthHostProvider.EnableProviderAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to login " + e.Message);
        }
        finally
        {
            authorizingInProgress = false;
        }

        if (AuthenticationStateProvider is EthereumAuthenticationStateProvider)
            ((EthereumAuthenticationStateProvider)AuthenticationStateProvider)?.NotifyStateHasChanged();

        await AuthState.AuthenticateAsync(User.Id, UserState.ChainId);
        StateHasChanged();
    }

    private async Task LoginToServer()
    {
        await AuthState.AuthenticateAsync(User.Id, UserState.ChainId);
    }
}