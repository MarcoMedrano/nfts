using System.Data;
using Application.Abstractions.Messaging;
using NftSample.Domain.Repositories;

namespace NftSample.Business.Users.Queries.GetById;

public sealed class GetUserQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async ValueTask<UserResponse> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(query.Id);

        return new (user.Id, user.Nickname, user.ProfilePicture);
    }
}