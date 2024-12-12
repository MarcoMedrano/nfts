namespace NftSample.IntegrationTests.UserRepository;

public class GetAllTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task GetAll_ReturnsAllUsersFromDatabase()
    {
        var userRepository = new Dal.UserRepository(fixture.Config);

        // Insert test data into the database
        await InsertUserData();

        // Act
        var results = await userRepository.GetAll();

        // Assert
        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.Equal(1, results.Count());

        var result = results.First();
        Assert.NotNull(result);
        Assert.Equal("testUserId", result.Id);
        Assert.Equal("TestUser", result.Nickname);
        Assert.Equal("ipfs://asdfas", result.ProfilePicture);
    }
}