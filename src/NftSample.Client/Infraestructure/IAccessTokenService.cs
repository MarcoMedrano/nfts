namespace NftSample.Client.Infraestructure;

public interface IAccessTokenService
{
    Task<string> GetAsync();

    Task SetAsync(string tokenValue);

    Task RemoveAsync();
}