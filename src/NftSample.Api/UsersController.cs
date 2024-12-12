using System.Runtime.CompilerServices;
using System.Windows.Input;
using NftSample.Api.Authorization;
using NftSample.Infraestructure.Api;
using NftSample.Business.Users.Queries.GetById;
using NftSample.Business.Users.Queries.GetNftsById;
using NftSample.Dtos.Nft;
using NftSample.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace NftSample.Api;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserCommands userCommands, IUserQueries queries, ILogger<UsersController> logger) : ControllerBase, IUsersController
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<UserDto?> GetById(string id) => (await queries.Query(new GetUserByIdQuery(id), CancellationToken.None)).Adapt<UserDto>();


    [HttpGet("{id}/nfts")]
    public async Task<IEnumerable<NftDto>?> GetNfts(string id, CancellationToken token)
    {
        var nftsResponse = await queries.Query(new GetNftsByUserIdQuery(id), token) as NftListsResponse;
        return nftsResponse.Nfts.Adapt<IEnumerable<NftDto>>();
    }

    [HttpPost]
    public async Task<string> Add(UserDto dto)
    {
        logger.LogDebug("Trying to add user {0}", dto.Id);
        this.ThrowIfNotAuthorized(dto.Id);
        return await userCommands.Send(dto.Adapt<CreateUserCommand>(), CancellationToken.None);
    }

    [HttpPatch]
    public async Task Update(UserDto dto)
    {
        logger.LogDebug("Trying to modify user {0}" , dto.Id);
        this.ThrowIfNotAuthorized(dto.Id);
        await userCommands.Send(dto.Adapt<UpdateUserCommand>(), CancellationToken.None);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        logger.LogDebug("Trying to delete user {0}", id);
        this.ThrowIfNotAuthorized(id);
        await userCommands.Send(new DeleteUserCommand(id), CancellationToken.None);
    }
}
