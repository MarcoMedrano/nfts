using System.Data;
using NftSample.Domain.Repositories;

namespace NftSample.Dal;

public class UserRepository(IConfiguration config) : IUserRepository
{
    public async Task<User> GetById(string id)
    {
        await using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        await using var command = new SqlCommand($"SELECT * FROM Users WHERE Id = @{nameof(User.Id)}", connection);
        command.Parameters.Add(new ($"@{nameof(User.Id)}", SqlDbType.NVarChar) { Value = id });

        await using var reader = await command.ExecuteReaderAsync();
        return (await reader.ReadAsync()) ? MapUserFromReader(reader) : null;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var users = new List<User?>();

        using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Users", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(MapUserFromReader(reader));
        }

        return users;
    }

    public async Task<string> Add(User user)
    {
        using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        using var command =
            new SqlCommand("INSERT INTO Users (Id, Nickname, ProfilePicture) VALUES (@Id, @Nickname, @ProfilePicture)",
                connection);
        command.Parameters.Add(new ($"@{nameof(User.Id)}", SqlDbType.NVarChar) { Value = user.Id });
        command.Parameters.Add(new ($"@{nameof(User.Nickname)}", SqlDbType.NVarChar) { Value = user.Nickname });
        command.Parameters.Add(new ($"@{nameof(User.ProfilePicture)}", SqlDbType.NVarChar) { Value = user.ProfilePicture });

        await command.ExecuteNonQueryAsync();
        return user.Id;
    }

    public async Task Update(User user)
    {
        using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        using var command =
            new SqlCommand("UPDATE Users SET Nickname = @Nickname, ProfilePicture = @ProfilePicture WHERE Id = @Id",
                connection);
        command.Parameters.Add(new ($"@{nameof(User.Id)}", SqlDbType.NVarChar) { Value = user.Id });
        command.Parameters.Add(new ($"@{nameof(User.Nickname)}", SqlDbType.NVarChar) { Value = user.Nickname });
        command.Parameters.Add(new ($"@{nameof(User.ProfilePicture)}", SqlDbType.NVarChar) { Value = user.ProfilePicture });

        await command.ExecuteNonQueryAsync();
    }

    public async Task Delete(string id)
    {
        using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        using var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
        command.Parameters.Add(new ($"@{nameof(User.Id)}", SqlDbType.NVarChar) { Value = id });

        await command.ExecuteNonQueryAsync();
    }

    private User? MapUserFromReader(SqlDataReader reader)
    {
        return new ()
        {
            Id = reader[nameof(User.Id)].ToString()!,
            Nickname = reader[nameof(User.Nickname)].ToString()!,
            ProfilePicture = reader[nameof(User.ProfilePicture)].ToString()!
        };
    }
}