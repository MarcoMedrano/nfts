using NftSample.Dal;
using Microsoft.EntityFrameworkCore;

namespace NftSample.Extensions;

public static class DbExtensions
{
    public static IServiceCollection AddANftSampleDbContext(this IServiceCollection services, IConfiguration config)
    {
        return services.AddDbContext<AppDbContext>(o => o.UseSqlServer(config["Database:ConnectionString"]));
    }

    public static IApplicationBuilder UseNftSampleDb(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if(app.Configuration["Database:GenerateTestData"] == "true") context.SeedData();
        }

        return app;
    }
}