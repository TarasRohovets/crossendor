using System.Text.Json;
using System.Text.Json.Serialization;
using Crossendor.Api.Domain;
using Dapper;

namespace Crossendor.Api.Data;

/// <summary>Shops in `builder.shops`: key columns for queries, the whole shop as jsonb in `data`.</summary>
public sealed class ShopStore(Db db)
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    public async Task<IReadOnlyList<Shop>> ListAsync(string ownerId, CancellationToken ct = default)
    {
        await using var conn = db.Connect();
        var rows = await conn.QueryAsync<string>(new CommandDefinition(
            "SELECT data::text FROM builder.shops WHERE owner_id = @ownerId ORDER BY created_at DESC LIMIT 200",
            new { ownerId }, cancellationToken: ct));
        return rows.Select(Read).ToList();
    }

    public async Task<Shop?> GetAsync(string ownerId, Guid id, CancellationToken ct = default)
    {
        await using var conn = db.Connect();
        var row = await conn.QuerySingleOrDefaultAsync<string>(new CommandDefinition(
            "SELECT data::text FROM builder.shops WHERE id = @id AND owner_id = @ownerId",
            new { id, ownerId }, cancellationToken: ct));
        return row is null ? null : Read(row);
    }

    public async Task SaveAsync(Shop shop, CancellationToken ct = default)
    {
        shop.UpdatedAt = DateTimeOffset.UtcNow;
        await using var conn = db.Connect();
        await conn.ExecuteAsync(new CommandDefinition("""
            INSERT INTO builder.shops (id, owner_id, status, source_url, data, created_at, updated_at)
            VALUES (@Id, @OwnerId, @Status, @SourceUrl, CAST(@Data AS jsonb), @CreatedAt, @UpdatedAt)
            ON CONFLICT (id) DO UPDATE SET status = EXCLUDED.status, data = EXCLUDED.data, updated_at = EXCLUDED.updated_at
            """,
            new
            {
                shop.Id, shop.OwnerId, Status = shop.Status.ToString(), shop.SourceUrl,
                Data = JsonSerializer.Serialize(shop, Json), shop.CreatedAt, shop.UpdatedAt,
            }, cancellationToken: ct));
    }

    private static Shop Read(string json) => JsonSerializer.Deserialize<Shop>(json, Json)!;
}
