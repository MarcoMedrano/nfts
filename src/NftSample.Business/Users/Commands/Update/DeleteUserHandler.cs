using Application.Abstractions.Messaging;
using NftSample.Domain.Abstractions;
using NftSample.Domain.Repositories;

namespace NftSample.Business;

public class DeleteUserHandler(IUserRepository usersRepo, IDbUnitOfWork dbUnitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async ValueTask Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await usersRepo.Delete(command.Id);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
