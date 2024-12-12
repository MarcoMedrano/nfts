using NftSample.Domain.Repositories;

namespace NftSample.Dal;

public class NftRepository(IConfiguration config) : INftRepository
{
    public async Task<Nft> GetById(long id)
    {
        await using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        await using var command = new SqlCommand("SELECT * FROM Nfts WHERE Id = @Id", connection);
        command.Parameters.AddWithValue($"@{nameof(Nft.Id)}", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync()) return MapNftFromReader(reader);

        return null; // Or throw an exception if you prefer
    }

    public async Task<IEnumerable<Nft>> GetByUserId(string userId)
    {
        await using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        await using var command = new SqlCommand("SELECT * FROM Nfts WHERE UserId = @Id", connection);
        command.Parameters.AddWithValue($"@{nameof(Nft.Id)}", userId);

        await using var reader = await command.ExecuteReaderAsync();
        var nfts = new List<Nft>();

        while (await reader.ReadAsync()) nfts.Add(MapNftFromReader(reader));

        return nfts;
    }

    public async Task<IEnumerable<Nft>> GetAll()
    {
        await using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        await using var command = new SqlCommand("SELECT * FROM Nfts", connection);

        await using var reader = await command.ExecuteReaderAsync();
        var nfts = new List<Nft>();

        while (await reader.ReadAsync()) nfts.Add(MapNftFromReader(reader));

        return nfts;
    }

    public async Task<long> Add(Nft entity)
    {
        await using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "INSERT INTO Nfts (UserId, Name, IpfsImage) VALUES (@UserId, @Name, @IpfsImage); SELECT SCOPE_IDENTITY();", connection);

        MapNftToParameters(command.Parameters, entity);
        var res = await command.ExecuteScalarAsync();
        return (long)Convert.ToUInt64(res);
    }

    public async Task Update(Nft entity)
    {
        await using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        await using var command = new SqlCommand(
            "UPDATE Nfts SET UserId = @UserId, Name = @Name, IpfsImage = @IpfsImage WHERE Id = @Id", connection);

        MapNftToParameters(command.Parameters, entity);

        await command.ExecuteNonQueryAsync();
    }

    public async Task Delete(long id)
    {
        await using var connection = new SqlConnection(config["Database:ConnectionString"]);
        await connection.OpenAsync();

        await using var command = new SqlCommand("DELETE FROM Nfts WHERE Id = @Id", connection);
        command.Parameters.AddWithValue($"@{nameof(Nft.Id)}", id);

        await command.ExecuteNonQueryAsync();
    }

    private static Nft MapNftFromReader(SqlDataReader reader)
    {
        int userIdIndex = reader.GetOrdinal(nameof(Nft.UserId));
        var userid = reader.IsDBNull(userIdIndex) ? string.Empty : reader.GetString(userIdIndex);
        return new()
        {
            Id = (long)reader[nameof(Nft.Id)],
            UserId = userid,
            Name = (string)reader[nameof(Nft.Name)],
            IpfsImage = (string)reader[nameof(Nft.IpfsImage)]
        };
    }

    private static void MapNftToParameters(SqlParameterCollection parameters, Nft entity)
    {
        parameters.AddWithValue($"@{nameof(Nft.Id)}", entity.Id);
        parameters.AddWithValue($"@{nameof(Nft.UserId)}", entity.UserId);
        parameters.AddWithValue($"@{nameof(Nft.Name)}", entity.Name);
        parameters.AddWithValue($"@{nameof(Nft.IpfsImage)}", entity.IpfsImage);
    }
}