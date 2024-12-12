using Application.Abstractions.Messaging;
using NftSample.Domain.Repositories;

namespace NftSample.Business.Users.Queries.GetNftsById;

public sealed class GetNftsByUserIdQueryHandler(INftRepository nftRepo) : IQueryHandler<GetNftsByUserIdQuery, NftListsResponse>
{
    public async ValueTask<NftListsResponse> Handle(
        GetNftsByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        var nfts = await nftRepo.GetByUserId(query.Id);

        if (nfts == null) return new(Array.Empty<NftResponse>());
        return new(nfts.Select(n => new NftResponse(n.Id, n.UserId, n.Name, n.IpfsImage)));
    }
}