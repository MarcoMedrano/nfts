namespace NftSample.IntegrationTests.NftRepository;

public class UpdateNftTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task Update_UpdatesNftInDatabase()
    {
        // Arrange
        await InsertNftData();
        var repository = new Dal.NftRepository(fixture.Config);

        // Act
        var nftToUpdate = await repository.GetById(1);
        nftToUpdate.UserId = "2";
        nftToUpdate.Name = "UpdatedTestNft1";
        await repository.Update(nftToUpdate);

        // Assert
        var updatedNft = await repository.GetById(1);
        Assert.NotNull(updatedNft);
        Assert.Equal("2", updatedNft.UserId);
        Assert.Equal("UpdatedTestNft1", updatedNft.Name);
    }
}