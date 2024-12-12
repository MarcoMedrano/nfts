
using NftSample.Dal;
using Microsoft.Extensions.Configuration;

namespace NftSample.IntegrationTests;

public class TestContainerFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; }
    public IConfiguration Config { get; }
    public AppDbContext Db { get; }

    public TestContainerFixture()
    {
        Container = new MsSqlBuilder()
            .WithPassword(Guid.NewGuid().ToString("D"))
            .Build();
        Config = new ConfigurationManager();
        Db = new (new(), Config);
    }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        var connection = Container.GetConnectionString();

        Config["Database:ConnectionString"] = connection;
        await Db.Database.EnsureCreatedAsync();
    }


    public async Task DisposeAsync()
    {
        await Db.DisposeAsync();
        await Container.StopAsync();
    }
}