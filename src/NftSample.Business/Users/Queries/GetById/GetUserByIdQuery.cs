using Application.Abstractions.Messaging;

namespace NftSample.Business.Users.Queries.GetById;

public sealed record GetUserByIdQuery(string Id) : IQuery<UserResponse>;
