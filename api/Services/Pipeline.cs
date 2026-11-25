using Crossendor.Api.Domain;

namespace Crossendor.Api.Services;

/// <summary>Who is asking. The open-source edition answers with its one local user; the cloud edition with the signed-in account.</summary>
public interface ICurrentUser
{
    /// <summary>The account id, or null when nobody is signed in.</summary>
    string? IdOf(HttpContext context);
}

/// <summary>Step 1: reads a product page (Shopify first, then AliExpress and Amazon).</summary>
public interface IProductImporter
{
    bool CanRead(Uri url);
    Task<ProductFacts> ReadAsync(Uri url, CancellationToken cancellationToken);
}

/// <summary>Step 2: writes the store around the product. The angle is an optional hint, such as the copy of an ad that sells it.</summary>
public interface IStoreWriter
{
    Task<StoreContent> WriteAsync(ProductFacts product, string? angle, CancellationToken cancellationToken);
}

/// <summary>Step 3: turns a finished shop into something Shopify takes (a theme ZIP and a product CSV, or a push to a connected store).</summary>
public interface IShopExporter
{
    string Kind { get; }
    Task<Stream> ExportAsync(Shop shop, CancellationToken cancellationToken);
}

public static class PipelineSetup
{
    public static void AddPipeline(this WebApplicationBuilder builder)
    {
        // Only placeholders exist so far. The real reader and writer replace these when they are built;
        // AI:UseMockData=true will keep the placeholders for trying the app without keys.
        builder.Services.AddSingleton<IProductImporter, MockProductImporter>();
        builder.Services.AddSingleton<IStoreWriter, MockStoreWriter>();
    }
}
