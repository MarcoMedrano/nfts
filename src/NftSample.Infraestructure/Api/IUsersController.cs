using NftSample.Dtos.Nft;
using NftSample.Dtos.User;
using Refit;

namespace NftSample.Infraestructure.Api;

public interface IUsersController
{
    [Get("/api/users/{id}")]
    Task<UserDto?> GetById(string id);

    [Post("/api/users")]
    Task<string> Add(UserDto dto);

    [Patch("/api/users")]
    Task Update(UserDto dto);

    [Delete("/api/users/{id}")]
    Task Delete(string id);

    [Get("/api/users/{id}/nfts")]
    Task<IEnumerable<NftDto>?> GetNfts(string id, CancellationToken cancellationToken);
}