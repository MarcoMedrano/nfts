using NftSample.Entities;

namespace NftSample.IntegrationTests.UserRepository;

// [Collection(nameof(CustomerApiFactoryTestCollection))]
public class UpdateUserTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task Update_UpdatesUserInDatabase()
    {
        var userRepository = new Dal.UserRepository(fixture.Config);

        // Insert test data into the database
        await InsertUserData();

        // Arrange
        var userToUpdate = new User
            { Id = "testUserId", Nickname = "UpdatedUser", ProfilePicture = "ipfs://updatedPicture" };

        // Act
        await userRepository.Update(userToUpdate);

        // Assert
        var result = await userRepository.GetById("testUserId");
        Assert.NotNull(result);
        Assert.Equal("testUserId", result.Id);
        Assert.Equal("UpdatedUser", result.Nickname);
        Assert.Equal("ipfs://updatedPicture", result.ProfilePicture);
    }
}