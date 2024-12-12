using NftSample.Api.Authorization;
using NftSample.Domain.Repositories;
using NftSample.Dal;
using NftSample.Dtos.Auth;
using NftSample.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Siwe;
using Nethereum.Siwe.Core;
using Nethereum.Util;

namespace NftSample.Api;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(SiweMessageService siweMessageService,
        ISiweJwtAuthorizationService siweJwtAuthService, IUserRepository db, ILogger<AuthenticationController> logger)
    : Controller
{
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest authenticateRequest)
    {
        var siweMessage = SiweMessageParser.Parse(authenticateRequest.SiweEncodedMessage);
        var signature = authenticateRequest.Signature;
        var validUser = await siweMessageService.IsUserAddressRegistered(siweMessage);

        if (!validUser)
        {
            logger.LogWarning("User {0} not found", siweMessage.Address);
            ModelState.AddModelError("Unauthorized", "Invalid User");
            return Unauthorized(ModelState);
        }

        if (!await siweMessageService.IsMessageSignatureValid(siweMessage, signature))
        {
            logger.LogWarning("The message signature for user {0} is not valid.", siweMessage.Address);

            ModelState.AddModelError("Unauthorized", "Invalid Signature");
            return Unauthorized(ModelState);
        }

        if (siweMessageService.IsMessageTheSameAsSessionStored(siweMessage))
        {
            if (siweMessageService.HasMessageDateStartedAndNotExpired(siweMessage))
            {
                var token = siweJwtAuthService.GenerateToken(siweMessage, signature);
                logger.LogDebug("User {0} authorized with jwt {1}", siweMessage.Address, token);
                return Ok(new AuthenticateResponse
                {
                    Address = siweMessage.Address,
                    Jwt = token
                });
            }
            logger.LogWarning("User {0} has an expired token", siweMessage.Address);

            ModelState.AddModelError("Unauthorized", "Expired token");
            return Unauthorized(ModelState);
        }

        logger.LogWarning("User {0} has no nonce", siweMessage.Address);
        ModelState.AddModelError("Unauthorized", "Matching Siwe message with nonce not found");
        return Unauthorized(ModelState);
    }

    [AllowAnonymous]
    [HttpPost("newsiwemessage")]
    public IActionResult GenerateNewSiweMessage(SiweMessageSeed msg)
    {
        SiweMessage message = new DevSiweMessage();
        message.Address = msg.Address.ToLower().ConvertToEthereumChecksumAddress();
        message.ChainId = msg.ChainId.ToString();
        message.Uri = msg.Uri;

        return Ok(siweMessageService.BuildMessageToSign(message));
    }

    [HttpPost("logout")]
    public IActionResult LogOut()
    {
        var siweMessage = SiweJwtMiddleware.GetSiweMessageFromContext(HttpContext);
        logger.LogInformation("User {0} logging out", siweMessage.Address);
        siweMessageService.InvalidateSession(siweMessage);
        return Ok();
    }

    [HttpGet("getuser")]
    public async Task<UserDto?> GetAuthenticatedUser()
    {
        //ethereum address
        var address = SiweJwtMiddleware.GetAddressFromContext(HttpContext);
        if (address == null) return null;

        var user = await db.GetById(address.ToLower());

        if (user == null) return null;
        return user.Adapt<UserDto>();
    }
}