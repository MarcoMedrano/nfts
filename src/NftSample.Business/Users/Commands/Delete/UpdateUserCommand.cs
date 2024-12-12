using Application.Abstractions.Messaging;

namespace NftSample.Business;

public sealed record UpdateUserCommand(string Id, string Nickname, string? ProfilePicture) : ICommand;