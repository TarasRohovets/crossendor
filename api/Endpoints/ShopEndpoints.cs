using Crossendor.Api.Data;
using Crossendor.Api.Domain;
using Crossendor.Api.Services;

namespace Crossendor.Api.Endpoints;

public static class ShopEndpoints
{
    public sealed record CreateShopRequest(string? Url, string? Angle);

    public static void MapShopEndpoints(this RouteGroupBuilder api)
    {
        var shops = api.MapGroup("/shops");

        shops.MapGet("/", async (HttpContext http, ICurrentUser user, ShopStore store, CancellationToken ct) =>
            user.IdOf(http) is { } owner ? Results.Ok(await store.ListAsync(owner, ct)) : Results.Unauthorized());

        shops.MapGet("/{id:guid}", async (Guid id, HttpContext http, ICurrentUser user, ShopStore store, CancellationToken ct) =>
        {
            if (user.IdOf(http) is not { } owner) return Results.Unauthorized();
            return await store.GetAsync(owner, id, ct) is { } shop ? Results.Ok(shop) : Results.NotFound();
        });

        // Paste a link: read the product, write the store, keep it as a draft.
        shops.MapPost("/", async (CreateShopRequest request, HttpContext http, ICurrentUser user, ShopStore store,
            IEnumerable<IProductImporter> importers, IStoreWriter writer, ILogger<Shop> logger, CancellationToken ct) =>
        {
            if (user.IdOf(http) is not { } owner) return Results.Unauthorized();
            if (!Uri.TryCreate(request.Url?.Trim(), UriKind.Absolute, out var url) || url.Scheme is not ("http" or "https"))
                return Results.BadRequest(new { error = "Paste a link to a product page, starting with https://." });
            var importer = importers.FirstOrDefault(i => i.CanRead(url));
            if (importer is null)
                return Results.BadRequest(new { error = "Links from this site can't be read yet. Paste a Shopify product link." });

            var shop = new Shop { OwnerId = owner, SourceUrl = url.ToString() };
            await store.SaveAsync(shop, ct);
            try
            {
                shop.Product = await importer.ReadAsync(url, ct);
                shop.Content = await writer.WriteAsync(shop.Product, request.Angle, ct);
                shop.Name = shop.Content.BrandName;
                shop.Status = ShopStatus.Draft;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "Shop {Id} failed for {Url}", shop.Id, url);
                shop.Status = ShopStatus.Failed;
                shop.Error = "The product couldn't be read from that page. Check the link and try again.";
            }
            await store.SaveAsync(shop, ct);
            return Results.Created($"/api/shops/{shop.Id}", shop);
        });
    }
}
