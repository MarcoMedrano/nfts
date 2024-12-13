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
        Assert.True(results.Count() > 1);

        var result = results.First();
        Assert.NotNull(result);
    }
}