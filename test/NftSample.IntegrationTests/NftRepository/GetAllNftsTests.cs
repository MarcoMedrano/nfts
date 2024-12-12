namespace NftSample.IntegrationTests.NftRepository;

public class GetAllNftsTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task GetAll_ReturnsAllNfts()
    {
        // Arrange
        await InsertNftData();
        var repository = new Dal.NftRepository(fixture.Config);

        // Act
        var nfts = await repository.GetAll();

        // Assert
        Assert.NotNull(nfts);
        Assert.NotEmpty(nfts);
    }
}