namespace Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, IResponse>
    where TQuery : IQuery<IResponse>
{
    ValueTask<IResponse> Handle(TQuery QUERY, CancellationToken cancellationToken);
}

public interface IResponse {}