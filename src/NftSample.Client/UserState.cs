using NftSample.Dtos.User;

namespace NftSample.Client;

public class UserState
{
    public UserDto User { get; set; } = new();
    public ulong ChainId { get; set; }
}