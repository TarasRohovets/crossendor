using Crossendor.Api.Domain;

namespace Crossendor.Api.Services;

/// <summary>Placeholder product: any link works, nothing is fetched.</summary>
public sealed class MockProductImporter : IProductImporter
{
    public bool CanRead(Uri url) => true;

    public Task<ProductFacts> ReadAsync(Uri url, CancellationToken cancellationToken) =>
        Task.FromResult(new ProductFacts(
            Title: "Sample product",
            Description: "A placeholder product. The real reader will take this from the page you linked.",
            Price: 29.90m,
            Currency: "USD",
            Images: [],
            Variants: ["Default"],
            SourceUrl: url.ToString()));
}

/// <summary>Placeholder store copy, so the flow can be clicked through without an AI key.</summary>
public sealed class MockStoreWriter : IStoreWriter
{
    public Task<StoreContent> WriteAsync(ProductFacts product, string? angle, CancellationToken cancellationToken) =>
        Task.FromResult(new StoreContent(
            BrandName: "Sample Store",
            Tagline: "One product, done well.",
            HomepageHeadline: $"Meet the {product.Title.ToLowerInvariant()}",
            ProductDescription: product.Description ?? "",
            Faq:
            [
                new FaqItem("How long does shipping take?", "Placeholder answer: the store writer will fill this in."),
                new FaqItem("Can I return it?", "Placeholder answer: the store writer will fill this in."),
            ],
            About: "Placeholder About page."));
}
