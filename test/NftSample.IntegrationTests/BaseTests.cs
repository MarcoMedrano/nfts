namespace NftSample.IntegrationTests;

public abstract class BaseTests(TestContainerFixture fixture) : IClassFixture<TestContainerFixture>
{
    protected async Task InsertUserData()
    {
        fixture.Db.Users.Add(new() { Id = "testUserId", Nickname = "TestUser", ProfilePicture = "ipfs://asdfas" });
        fixture.Db.Users.Add(new() { Id = "1", Nickname = "TestUser1", ProfilePicture = "ipfs://asdfas" });
        fixture.Db.Users.Add(new() { Id = "2", Nickname = "TestUser1", ProfilePicture = "ipfs://asdfas" });
        await fixture.Db.SaveChangesAsync();
    }

    protected async Task InsertNftData()
    {
        // requirement cause foreign keys
        await InsertUserData();
        // Insert sample Nft data
        fixture.Db.Nfts.Add(new ()
        {
            UserId = "1",
            Name = "TestNft1",
            IpfsImage = "ipfs://testimage1"
        });

        fixture.Db.Nfts.Add(new ()
        {
            UserId = "1",
            Name = "TestNft2",
            IpfsImage = "ipfs://testimage2"
        });

        fixture.Db.Nfts.Add(new ()
        {
            UserId = "2",
            Name = "TestNft3",
            IpfsImage = "ipfs://testimage3"
        });

        await fixture.Db.SaveChangesAsync();
    }
}