using NftSample.Domain.Repositories;
using NftSample.Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Nethereum.Siwe.Core;

namespace NftSample.Api.Authorization;

public class SiweJwtMiddleware(RequestDelegate next)
{
    public const string ContextEthereumAddress = "ethereumAddress";
    public const string ContextSiweMessage = "siweMessage";

    public static string? GetAddressFromContext(HttpContext context)
    {
        if (context.Items.ContainsKey(ContextEthereumAddress)) return (string)context.Items[ContextEthereumAddress]!;

        return null;
    }

    public static SiweMessage? GetSiweMessageFromContext(HttpContext context)
    {
        if (context.Items.ContainsKey(ContextSiweMessage)) return (SiweMessage)context.Items[ContextSiweMessage]!;

        return null;
    }

    public static void SetSiweMessage(HttpContext context, SiweMessage siweMessage)
    {
        context.Items[ContextSiweMessage] = siweMessage;
    }

    public static void SetEthereumAddress(HttpContext context, string address)
    {
        context.Items[ContextEthereumAddress] = address.ToLower();
    }

    public async Task InvokeAsync(HttpContext context, ISiweJwtAuthorizationService siweJwtAuthorization, IUserRepository userRepo, ILogger<SiweJwtMiddleware> loggger)
    {
        var authHeader = context.Request.Headers["Authorization"];

        var token = authHeader.FirstOrDefault()?.Split(" ").Last();
        loggger.LogDebug("Token found {0}", token);
        string baseUrl = new Uri(context.Request.GetEncodedUrl()).GetLeftPart(UriPartial.Authority);

        var siweMessage = await siweJwtAuthorization.ValidateToken(token, baseUrl);
        if (siweMessage != null)
        {
            loggger.LogDebug("Token valid, setting the user in our DB");
            SetEthereumAddress(context, siweMessage.Address);
            SetSiweMessage(context, siweMessage);
            await SetNewUser(siweMessage.Address, userRepo);
        }

        await next(context);
    }

    private async Task SetNewUser(string userAddress, IUserRepository userRepo)
    {
        var user = await userRepo.GetById(userAddress);
        if (user == null) await userRepo.Add(new() { Id = userAddress, Nickname = "Your nickname here.", ProfilePicture = string.Empty});
    }
}