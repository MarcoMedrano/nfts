using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Nethereum.Blazor;
using Nethereum.Siwe.Core;
using Nethereum.UI;
using Nethereum.Util;

namespace NftSample.Client.Infraestructure;

public class SiweAuthenticationStateProvider(SiweApiUserLoginService userLoginService,
        IAccessTokenService accessTokenService,
        SelectedEthereumHostProviderService selectedHostProviderService,
        UserState userState)
    : EthereumAuthenticationStateProvider(selectedHostProviderService)
{
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetUserAsync();

        if (currentUser != null && currentUser.Id != null)
        {
            userState.User = currentUser;
            //create claimsPrincipal
            var claimsPrincipal = GenerateSiweClaimsPrincipal(currentUser);
            return new (claimsPrincipal);
        }

        // await _accessTokenService.RemoveAsync();
        return await base.GetAuthenticationStateAsync();
    }

    public async Task AuthenticateAsync(string address, ulong chainId)
    {
        if (EthereumHostProvider == null || !EthereumHostProvider.Available)
        {
            throw new ("Cannot authenticate user, an Ethereum host is not available");
        }

        var siweMessage = await userLoginService.GenerateNewSiweMessage(address.ToLower(), chainId);
        var signedMessage = await EthereumHostProvider.SignMessageAsync(siweMessage);
        await AuthenticateAsync(SiweMessageParser.Parse(siweMessage), signedMessage);
    }

    public async Task AuthenticateAsync(SiweMessage siweMessage, string signature)
    {
        var authenticateResponse = await userLoginService.Authenticate(siweMessage, signature);
        if (authenticateResponse.Jwt != null && authenticateResponse.Address.IsTheSameAddress(siweMessage.Address))
        {
            await accessTokenService.SetAsync(authenticateResponse.Jwt);
            await MarkUserAsAuthenticated();
        }
        else
        {
            throw new ("Invalid authentication response");
        }
    }

    public async Task MarkUserAsAuthenticated()
    {
        var user = await GetUserAsync();
        if (user == null) throw new ("User null as token not found.");

        userState.User = user;
        Console.WriteLine("SET user " + user.Nickname);
        var claimsPrincipal = GenerateSiweClaimsPrincipal(user);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task LogOutUserAsync()
    {
        Console.WriteLine("SiweAuthenticationStateProvider.LogOutUserAsync");
        var token = await accessTokenService.GetAsync();
        await accessTokenService.RemoveAsync();
        try
        {
            await userLoginService.Logout(token);
        }
        catch
        {
            // remove from the server but catch gracefully as the token is gone from the state
        }

        var authenticationState = await base.GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    }

    public async Task<UserDto> GetUserAsync()
    {
        var jwtToken = await accessTokenService.GetAsync();
        if (jwtToken == null) return null;
        return await userLoginService.GetUser(jwtToken);
    }

    public async Task<bool> IsServerAuthenticated()
    {
        var jwtToken = await accessTokenService.GetAsync();
        if (string.IsNullOrEmpty(jwtToken)) return false;

        var state = await GetAuthenticationStateAsync();
        var inRole = state.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == Roles.ServerAuthenticated);

        return inRole && state.User.Identity is { IsAuthenticated: true };
    }

    private ClaimsPrincipal GenerateSiweClaimsPrincipal(UserDto currentUser)
    {
        ArgumentException.ThrowIfNullOrEmpty(currentUser.Nickname);

        //create a claims
        var claimName = new Claim(ClaimTypes.Name, currentUser.Nickname);
        var claimEthereumAddress = new Claim(ClaimTypes.NameIdentifier, currentUser.Id);
        var claimEthereumConnectedRole = new Claim(ClaimTypes.Role, Roles.WalletAuthenticated);
        var claimSiweAuthenticatedRole = new Claim(ClaimTypes.Role, Roles.ServerAuthenticated);

        //create claimsIdentity
        var claimsIdentity =
            new ClaimsIdentity(
                new[] { claimEthereumAddress, claimName, claimEthereumConnectedRole, claimSiweAuthenticatedRole },
                "siweAuth");
        //create claimsPrincipal
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }
}