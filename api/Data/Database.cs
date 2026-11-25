using Dapper;
using Npgsql;

namespace Crossendor.Api.Data;

/// <summary>PostgreSQL connections (`ConnectionStrings:PostgresConnection`), used with Dapper.</summary>
public sealed class Db
{
    private readonly string _connectionString;

    public Db(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PostgresConnection") ?? "";
        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new InvalidOperationException(
                "Set the PostgreSQL connection string (ConnectionStrings:PostgresConnection, env ConnectionStrings__PostgresConnection).");
    }

    public NpgsqlConnection Connect() => new(_connectionString);
}

public static class DbSetup
{
    public static void AddAppDatabase(this WebApplicationBuilder builder)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        builder.Services.AddSingleton<Db>();
        builder.Services.AddSingleton<ShopStore>();
    }

    /// <summary>
    /// Runs every migrations/*.sql in name order. Each file must be safe to run twice, and touches only the
    /// `builder` schema: Crossendor Cloud runs these same files against a database other apps also use.
    /// </summary>
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        var db = app.Services.GetRequiredService<Db>();
        var folder = Path.Combine(AppContext.BaseDirectory, "migrations");
        await using var conn = await OpenWhenReadyAsync(db, app.Logger);
        foreach (var file in Directory.GetFiles(folder, "*.sql").Order(StringComparer.Ordinal))
            await conn.ExecuteAsync(await File.ReadAllTextAsync(file));
        app.Logger.LogInformation("Database ready.");
    }

    /// <summary>The database container may still be starting when the API does (Docker Compose): wait up to a minute.</summary>
    private static async Task<NpgsqlConnection> OpenWhenReadyAsync(Db db, ILogger logger)
    {
        for (var attempt = 1; ; attempt++)
        {
            var conn = db.Connect();
            try
            {
                await conn.OpenAsync();
                return conn;
            }
            catch (Exception ex) when (attempt < 20 && ex is NpgsqlException or System.Net.Sockets.SocketException)
            {
                await conn.DisposeAsync();
                if (attempt == 1) logger.LogInformation("Waiting for PostgreSQL…");
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}
