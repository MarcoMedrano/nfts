using NftSample.Api.Authorization;
using NftSample.Domain.Repositories;
using NftSample.Business;
using NftSample.Dal;
using Nethereum.Siwe;
using NftSample.Domain.Abstractions;
using NftSample.Business.Users.Queries.GetById;
using NftSample.Business.Users.Queries.GetNftsById;

namespace NftSample.Extensions;

public static class NftSampleServicesExtensions
{
    public static IServiceCollection AddNftSampleServices(this IServiceCollection services)
    {
        services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INftRepository, NftRepository>();

        services.AddScoped<GetUserQueryHandler, GetUserQueryHandler>();
        services.AddScoped<CreateUserHandler, CreateUserHandler>();
        services.AddScoped<UpdateUserHandler, UpdateUserHandler>();
        services.AddScoped<DeleteUserHandler, DeleteUserHandler>();

        services.AddScoped<GetNftsByUserIdQueryHandler, GetNftsByUserIdQueryHandler>();

        services.AddScoped<IUserCommands, UserCommands>();
        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<INftService, NftService>();


        // Authorization
        services.AddSingleton<ISessionStorage, InMemorySessionNonceStorage>();
        services.AddScoped<SiweMessageService>();
        services.AddScoped<ISiweJwtAuthorizationService, SiweJwtAuthorizationService>();

        return services;
    }
}