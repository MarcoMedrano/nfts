using Application.Abstractions.Messaging;

namespace NftSample.Business;

public sealed record DeleteUserCommand(string Id) : ICommand;