using Application.Abstractions.Messaging;

namespace NftSample.Business.Users.Queries.GetNftsById;


public sealed record NftListsResponse(IEnumerable<NftResponse> Nfts) : IResponse;
public sealed record NftResponse(long Id, string UserId, string Name, string IpfsImage);