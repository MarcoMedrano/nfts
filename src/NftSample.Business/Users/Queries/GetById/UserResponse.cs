using Application.Abstractions.Messaging;

namespace NftSample.Business.Users.Queries.GetById;

public sealed record UserResponse(string Id, string Nickname, string? ProfilePicture) : IResponse;