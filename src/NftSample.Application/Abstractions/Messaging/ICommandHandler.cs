namespace Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    ValueTask Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    new ValueTask<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}
