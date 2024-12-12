
using NftSample.Domain.Abstractions;
using NftSample.Domain.Repositories;
using NftSample.Business.Users.Queries.GetById;
using NftSample.Business.Users.Queries.GetNftsById;


namespace NftSample.UnitTests;

public class UserCqrsTests
{

    [Fact]
    public async Task GetById_ValidId_ReturnsUser()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(r => r.GetById("validId")).ReturnsAsync(new User { Id = "validId", Nickname = "JohnDoe" });

        var getUserQueryHandler = new GetUserQueryHandler(mockUserRepository.Object);
        var userQueries = new UserQueries(getUserQueryHandler, null);

        // Act
        var result = await userQueries.Query(new GetUserByIdQuery("validId"), CancellationToken.None) as UserResponse;

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("validId");
        result.Nickname.Should().Be("JohnDoe");
    }

    [Fact]
    public async Task GetNftsById_ValidId_ReturnsNfts()
    {
        // Arrange
        var mockNftRepository = new Mock<INftRepository>();
        mockNftRepository.Setup(r => r.GetByUserId("validId")).ReturnsAsync([new Nft { Id = 1, Name = "Cat's and roses" }]);

        var queryHandler = new GetNftsByUserIdQueryHandler(mockNftRepository.Object);
        var userQueries = new UserQueries(null, queryHandler);

        // Act
        var result = await userQueries.Query(new GetNftsByUserIdQuery("validId"), CancellationToken.None) as NftListsResponse;

        // Assert
        result.Should().NotBeNull();
        result.Nfts.Should().NotBeEmpty();
        result.Nfts.Should().Contain(e => e.Id == 1);
    }

    [Fact]
    public async Task Add_ValidUser_CallsRepositoryAdd()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        IUserCommands userCommands = BuildUserCommands(mockUserRepository.Object);
        var newUser = new CreateUserCommand ("newUserId", "NewUser", null);

        // Act
        await userCommands.Send(newUser, CancellationToken.None);

        // Assert
        mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Update_ValidUser_CallsRepositoryUpdate()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        IUserCommands userCommands = BuildUserCommands(mockUserRepository.Object);
        var updateCommand = new UpdateUserCommand ("existingUserId", "ExistingUser", null);

        // Act
        await userCommands.Send(updateCommand, CancellationToken.None);

        // Assert
        mockUserRepository.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ValidId_CallsRepositoryDelete()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        IUserCommands userCommands = BuildUserCommands(mockUserRepository.Object);
        var deleteCommand = new DeleteUserCommand ("deleteUserId");

        // Act
        await userCommands.Send(deleteCommand, CancellationToken.None);

        // Assert
        mockUserRepository.Verify(r => r.Delete("deleteUserId"), Times.Once);
    }

    private IUserCommands BuildUserCommands(IUserRepository userRepository)
    {
        IDbUnitOfWork unitOfWork = new Mock<IDbUnitOfWork>().Object;
        return new UserCommands(new(userRepository, unitOfWork), new(userRepository, unitOfWork), new(userRepository, unitOfWork));
    }
}