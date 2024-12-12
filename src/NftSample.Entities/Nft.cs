namespace NftSample.Entities;

public class Nft
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public string Name { get; set; }
    public string IpfsImage { get; set; }
}
