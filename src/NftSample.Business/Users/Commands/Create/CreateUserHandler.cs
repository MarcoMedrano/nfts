using Application.Abstractions.Messaging;
using NftSample.Domain.Abstractions;
using NftSample.Domain.Repositories;
using NftSample.Entities;

namespace NftSample.Business;

public class CreateUserHandler(IUserRepository usersRepo, IDbUnitOfWork dbUnitOfWork) : ICommandHandler<CreateUserCommand, string>
{
    public async ValueTask<string> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        User user = new() { Id = command.Id, Nickname = command.Nickname, ProfilePicture = command.ProfilePicture };

        await usersRepo.Add(user);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
