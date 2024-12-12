using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Siwe;
using Nethereum.Siwe.Core;

namespace NftSample.Api.Authorization;

public class SiweJwtAuthorizationService(IOptions<AppSettings> appSettings, SiweMessageService siweMessageService,
        IHostEnvironment env)
    : ISiweJwtAuthorizationService
{
    private const string ClaimTypeAddress = "address";
    private const string ClaimTypeChainId = "chainId";
    private const string ClaimTypeNonce = "nonce";
    private const string ClaimTypeSignature = "signature";
    private const string ClaimTypeSiweExpiry = "siweExpiry";
    private const string ClaimTypeSiweIssuedAt = "siweIssueAt";
    private const string ClaimTypeSiweNotBefore = "siweNotBefore";

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IHostEnvironment _env = env;

    public string GenerateToken(SiweMessage siweMessage, string signature)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(new[]
            {
                new Claim(ClaimTypeAddress, siweMessage.Address),
                new Claim(ClaimTypeChainId, siweMessage.ChainId),
                new Claim(ClaimTypeNonce, siweMessage.Nonce),
                new Claim(ClaimTypeSignature, signature),
                new Claim(ClaimTypeSiweExpiry, siweMessage.ExpirationTime),
                new Claim(ClaimTypeSiweIssuedAt, siweMessage.IssuedAt),
                new Claim(ClaimTypeSiweNotBefore, siweMessage.NotBefore)
            }),


            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        if (!string.IsNullOrEmpty(siweMessage.ExpirationTime))
            tokenDescriptor.Expires = GetIso8602AsDateTime(siweMessage.ExpirationTime);
        if (!string.IsNullOrEmpty(siweMessage.IssuedAt))
            tokenDescriptor.IssuedAt = GetIso8602AsDateTime(siweMessage.IssuedAt);
        if (!string.IsNullOrEmpty(siweMessage.NotBefore))
            tokenDescriptor.NotBefore = GetIso8602AsDateTime(siweMessage.NotBefore);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<SiweMessage?> ValidateToken(string token, string baseUrl)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            string address = jwtToken.Claims.First(x => x.Type == ClaimTypeAddress).Value;
            string chainId = jwtToken.Claims.First(x => x.Type == ClaimTypeChainId).Value;
            string nonce = jwtToken.Claims.First(x => x.Type == ClaimTypeNonce).Value;
            string issuedAt = jwtToken.Claims.First(x => x.Type == ClaimTypeSiweIssuedAt).Value;
            string expiry = jwtToken.Claims.First(x => x.Type == ClaimTypeSiweExpiry).Value;
            string notBefore = jwtToken.Claims.First(x => x.Type == ClaimTypeSiweNotBefore).Value;


            string signature = jwtToken.Claims.First(x => x.Type == ClaimTypeSignature).Value;


            SiweMessage siweMessage = new DevSiweMessage();
            siweMessage.Address = address;
            siweMessage.ChainId = chainId;
            siweMessage.Nonce = nonce;
            siweMessage.ExpirationTime = expiry;
            siweMessage.IssuedAt = issuedAt;
            siweMessage.NotBefore = notBefore;
            siweMessage.Uri = baseUrl;

            if (await siweMessageService.IsMessageSignatureValid(siweMessage, signature))
                if (siweMessageService.IsMessageTheSameAsSessionStored(siweMessage))
                    if (siweMessageService.HasMessageDateStartedAndNotExpired(siweMessage))
                        return siweMessage;

            return null;
        }
        catch (Exception ex)
        {
            // return null if validation fails
            return null;
        }
    }

    protected DateTime GetIso8602AsDateTime(string iso8601dateTime)
    {
        return DateTime.ParseExact(iso8601dateTime, "o",
            CultureInfo.InvariantCulture).ToUniversalTime();
    }
}