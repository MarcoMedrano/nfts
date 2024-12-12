using NftSample.Domain.Repositories.Base;
using NftSample.Entities;

namespace NftSample.Domain.Repositories;

public interface INftRepository : IRepository<Nft, long>
{
    Task<IEnumerable<Nft>?> GetByUserId(string userId);
}