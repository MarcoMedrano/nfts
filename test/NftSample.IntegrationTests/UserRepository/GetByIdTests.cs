namespace NftSample.IntegrationTests.UserRepository;

public class GetByIdTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task GetById_ValidId_ReturnsUserFromDatabase()
    {
        var userRepository = new Dal.UserRepository(fixture.Config);

        // Insert test data into the database
        await InsertUserData();

        // Act
        var result = await userRepository.GetById("testUserId");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testUserId", result.Id);
        Assert.Equal("TestUser", result.Nickname);
        Assert.Equal("ipfs://asdfas", result.ProfilePicture);
    }
}