using NftSample.Domain.Abstractions;

namespace NftSample.Dal;

public class DbUnitOfWork(AppDbContext db) : IDbUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => db.SaveChangesAsync(cancellationToken);
}
