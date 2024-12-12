namespace NftSample.IntegrationTests.UserRepository;

public class DeleteUserTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task Delete_RemovesUserFromDatabase()
    {
        var userRepository = new Dal.UserRepository(fixture.Config);

        // Insert test data into the database
        await InsertUserData();

        // Act
        await userRepository.Delete("testUserId");

        // Assert
        var result = await userRepository.GetById("testUserId");
        Assert.Null(result);
    }
}