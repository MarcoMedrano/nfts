using Application.Abstractions.Messaging;

namespace NftSample.Business;

public sealed record CreateUserCommand(string Id, string Nickname, string? ProfilePicture) : ICommand<string>;
