namespace NftSample.IntegrationTests.NftRepository;

public class DeleteNftTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task Delete_RemovesNftsFromDatabase()
    {
        // Arrange
        await InsertNftData();
        var repository = new Dal.NftRepository(fixture.Config);

        // Act
        await repository.Delete(1);

        // Assert
        var deletedNft = await repository.GetById(1);
        Assert.Null(deletedNft);
    }
}