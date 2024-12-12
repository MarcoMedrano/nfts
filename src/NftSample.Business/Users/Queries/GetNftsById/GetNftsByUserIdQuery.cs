using Application.Abstractions.Messaging;

namespace NftSample.Business.Users.Queries.GetNftsById;

public sealed record GetNftsByUserIdQuery(string Id) : IQuery<NftListsResponse>;
