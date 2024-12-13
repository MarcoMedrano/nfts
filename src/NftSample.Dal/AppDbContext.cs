using Microsoft.EntityFrameworkCore;

namespace NftSample.Dal;

public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Nft> Nfts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(config["Database:ConnectionString"]);
    }

    public void SeedData()
    {
        this.Users.AddRange(
        new User { Id = "1", Nickname = "User1", ProfilePicture = "/images/MellowFawn.PNG" }
        );

        this.Nfts.AddRange(
            new Nft { Name = "Bam", IpfsImage = "http://localhost:8080/images/ChillBam.PNG" },
            new Nft { Name = "MellowFawn", IpfsImage = "http://localhost:8080/images/MellowFawn.PNG" },
            new Nft { Name = "Tranqx", IpfsImage = "http://localhost:8080/images/Tranqx.PNG" },
            new Nft { Name = "Xenit", IpfsImage = "http://localhost:8080/images/XenithGuardian.PNG" },
            new Nft { Name = "ZenDeer", IpfsImage = "http://localhost:8080/images/ZenDeer.PNG" }
        );

        this.SaveChanges();
    }
}