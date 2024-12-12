using System.Net.Http.Headers;

namespace NftSample.Client.Extensions;

public class BearerTokenHandler(IAccessTokenService tokenStore) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await tokenStore.GetAsync());
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}