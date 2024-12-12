namespace NftSample.Domain.Abstractions;

public interface IDbUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}