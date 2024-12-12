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
            new Nft { Name = "Nik", IpfsImage = "/images/ChillBam.PNG" },
            new Nft { Name = "Guns N'cats", IpfsImage = "/images/XenithGuardian.PNG" }
        );

        this.SaveChanges();
    }
}