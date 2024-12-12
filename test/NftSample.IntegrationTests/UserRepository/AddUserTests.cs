using NftSample.Entities;

namespace NftSample.IntegrationTests.UserRepository;

public class AddUserTests(TestContainerFixture fixture) : BaseTests(fixture)
{
    [Fact]
    public async Task Add_AddsUserToDatabase()
    {
        var userRepository = new Dal.UserRepository(fixture.Config);

        // Arrange
        var userToAdd = new User { Id = "newUserId", Nickname = "NewUser", ProfilePicture = "ipfs://newUserPicture" };

        // Act
        await userRepository.Add(userToAdd);

        // Assert
        var result = await userRepository.GetById("newUserId");
        Assert.NotNull(result);
        Assert.Equal("newUserId", result.Id);
        Assert.Equal("NewUser", result.Nickname);
        Assert.Equal("ipfs://newUserPicture", result.ProfilePicture);
    }
}