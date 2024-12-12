using Application.Abstractions.Messaging;

namespace NftSample.Business;

public interface IUserCommands
{
    ValueTask<string> Send(ICommand<string> command, CancellationToken cancellationToken);
    ValueTask Send(ICommand command, CancellationToken cancellationToken);
}

public class UserCommands(CreateUserHandler createUserHandler, UpdateUserHandler updateUserHandler, DeleteUserHandler deleteUserHandler) : IUserCommands
{
    public async ValueTask<string> Send(ICommand<string> command, CancellationToken cancellationToken)
    {
        return command switch 
        {
            CreateUserCommand createCommand => await createUserHandler.Handle(createCommand, cancellationToken),
            _ => throw new NotImplementedException(),
        };
    }

    public async ValueTask Send(ICommand command, CancellationToken cancellationToken)
    {
        var task = command switch
        {
            UpdateUserCommand updateCommand => updateUserHandler.Handle(updateCommand, cancellationToken),
            DeleteUserCommand deleteCommand => deleteUserHandler.Handle(deleteCommand, cancellationToken),
            _ => throw new NotImplementedException(),
        };

        await task;
    }
}