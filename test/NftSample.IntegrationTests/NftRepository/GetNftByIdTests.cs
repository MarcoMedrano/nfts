namespace NftSample.IntegrationTests.NftRepository;

public class GetNftByIdTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task GetById_ValidId_ReturnsNftFromDatabase()
    {
        // Arrange
        await InsertNftData();
        var repository = new Dal.NftRepository(fixture.Config);

        // Act
        var nft = await repository.GetById(1);

        // Assert
        Assert.NotNull(nft);
        Assert.Equal(1, nft.Id);
        Assert.Equal("1", nft.UserId);
        Assert.Equal("TestNft1", nft.Name);
    }
}