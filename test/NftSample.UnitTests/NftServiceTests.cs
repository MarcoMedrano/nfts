
using NftSample.Domain.Repositories;

namespace NftSample.UnitTests;

public class NftServiceTests
{
    [Fact]
    public async Task GetById_ValidId_ReturnsNft()
    {
        // Arrange
        var mockNftRepository = new Mock<INftRepository>();
        mockNftRepository.Setup(r => r.GetById(1)).ReturnsAsync(new Nft { Id = 1, IpfsImage = "ipfs://image" });

        var nftService = new NftService(mockNftRepository.Object);

        // Act
        var result = await nftService.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("ipfs://image", result.IpfsImage);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfNfts()
    {
        // Arrange
        var mockNftRepository = new Mock<INftRepository>();
        mockNftRepository.Setup(r => r.GetAll()).ReturnsAsync(new List<Nft>
        {
            new Nft { Id = 1, UserId = "1", IpfsImage = "ipfs://image1" },
            new Nft { Id = 2, UserId = "1", IpfsImage = "ipfs://image2" }
        });

        var nftService = new NftService(mockNftRepository.Object);

        // Act
        var result = await nftService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Nft>>(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Add_ValidNft_CallsRepositoryAdd()
    {
        // Arrange
        var mockNftRepository = new Mock<INftRepository>();
        var nftService = new NftService(mockNftRepository.Object);

        var newNft = new Nft { Id = 3, UserId = "1", IpfsImage = "ipfs://image3" };

        // Act
        await nftService.Add(newNft);

        // Assert
        mockNftRepository.Verify(r => r.Add(newNft), Times.Once);
    }

    [Fact]
    public async Task Update_ValidNft_CallsRepositoryUpdate()
    {
        // Arrange
        var mockNftRepository = new Mock<INftRepository>();
        var nftService = new NftService(mockNftRepository.Object);

        var existingNft = new Nft { Id = 4, UserId = "1", IpfsImage = "ipfs://image4" };

        // Act
        await nftService.Update(existingNft);

        // Assert
        mockNftRepository.Verify(r => r.Update(existingNft), Times.Once);
    }

    [Fact]
    public async Task Delete_ValidId_CallsRepositoryDelete()
    {
        // Arrange
        var mockNftRepository = new Mock<INftRepository>();
        var nftService = new NftService(mockNftRepository.Object);

        // Act
        await nftService.Delete(5);

        // Assert
        mockNftRepository.Verify(r => r.Delete(5), Times.Once);
    }
}