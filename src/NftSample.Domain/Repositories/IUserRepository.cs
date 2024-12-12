using NftSample.Domain.Repositories.Base;
using NftSample.Entities;

namespace NftSample.Domain.Repositories;

public interface IUserRepository : IRepository<User, string>
{
}