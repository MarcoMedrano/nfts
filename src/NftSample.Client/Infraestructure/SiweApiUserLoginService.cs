using System.Net.Http.Json;
using NftSample.Dtos.Auth;
using Nethereum.Siwe.Core;

namespace NftSample.Client.Infraestructure;

public class SiweApiUserLoginService(HttpClient httpClient)
{
    public async Task<string> GenerateNewSiweMessage(string address, ulong chainId)
    {
        var baseUrl = httpClient.BaseAddress.ToString();
        int index = baseUrl.LastIndexOf('/');
        if (index >= 0) baseUrl = baseUrl.Remove(index, 1);

        var message = new SiweMessageSeed { Address = address, ChainId = chainId, Uri = baseUrl};
        var response = await httpClient.PostAsJsonAsync("api/Authentication/newsiwemessage", message);

        return await response.Content.ReadAsStringAsync();
    }


    public async Task<AuthenticateResponse> Authenticate(SiweMessage siweMessage, string signature)
    {
        var siweMessageEncoded = SiweMessageStringBuilder.BuildMessage(siweMessage);
        var request = new AuthenticateRequest()
        {
            SiweEncodedMessage = siweMessageEncoded,
            Signature = signature
        };

        var response = await httpClient.PostAsJsonAsync("api/Authentication/authenticate", request);

        return await response.Content.ReadFromJsonAsync<AuthenticateResponse>();
    }

    public async Task<UserDto> GetUser(string token)
    {
        try
        {
            var user = await httpClient.GetAsync<UserDto>("api/Authentication/getuser", token);
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType());
            return null;
        }
    }

    public async Task Logout(string token)
    {
        try
        {
            await httpClient.PostAsync("api/Authentication/logout", null, token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType());
            throw;
        }
    }
}