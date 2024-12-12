using NftSample.Domain.Repositories;
using NftSample.Dal;
using NftSample.Entities;

namespace NftSample.Business;

public class NftService(INftRepository repo) : INftService
{
    public Task<Nft> GetById(long id) => repo.GetById(id);

    public Task<IEnumerable<Nft>> GetAll() => repo.GetAll();

    public Task<long> Add(Nft nft) => repo.Add(nft);

    public Task Update(Nft nft) => repo.Update(nft);

    public Task Delete(long id) => repo.Delete(id);
}
