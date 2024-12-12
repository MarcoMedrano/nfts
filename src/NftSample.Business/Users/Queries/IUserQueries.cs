using Application.Abstractions.Messaging;
using NftSample.Business.Users.Queries.GetById;
using NftSample.Business.Users.Queries.GetNftsById;

namespace NftSample.Business;

public interface IUserQueries
{
    ValueTask<IResponse> Query<TQuery>(TQuery query, CancellationToken cancellationToken);
}


public class UserQueries(GetUserQueryHandler getUserQueryHandler, GetNftsByUserIdQueryHandler getNftsByUserIdQueryHandler) : IUserQueries
{
    public async ValueTask<IResponse> Query<TQuery>(TQuery query, CancellationToken cancellationToken) => query switch
    {
        GetUserByIdQuery _query => await getUserQueryHandler.Handle(_query, cancellationToken),
        GetNftsByUserIdQuery _query => await getNftsByUserIdQueryHandler.Handle(_query, cancellationToken),
        _ => throw new NotImplementedException(),
    };
}