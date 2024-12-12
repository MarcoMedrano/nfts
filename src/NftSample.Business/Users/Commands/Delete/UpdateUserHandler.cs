using Application.Abstractions.Messaging;
using NftSample.Domain.Abstractions;
using NftSample.Domain.Repositories;
using NftSample.Entities;

namespace NftSample.Business;

public class UpdateUserHandler(IUserRepository usersRepo, IDbUnitOfWork dbUnitOfWork) : ICommandHandler<UpdateUserCommand>
{
    public async ValueTask Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        User user = new() { Id = command.Id, Nickname = command.Nickname, ProfilePicture = command.ProfilePicture };

        await usersRepo.Update(user);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
