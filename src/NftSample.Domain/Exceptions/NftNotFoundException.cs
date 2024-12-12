using NftSample.Domain.Exceptions.Base;

namespace NftSample.Domain.Exceptions;

public class NftNotFoundException : NotFoundException
{
    public NftNotFoundException(long id) : base($"The Nft with id '{id}' was not found.")
    {
    }
}