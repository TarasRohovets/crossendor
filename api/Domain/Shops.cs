namespace Crossendor.Api.Domain;

public enum ShopStatus { Writing, Draft, Exported, Failed }

/// <summary>One store being built from one product.</summary>
public sealed class Shop
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string OwnerId { get; init; }
    public required string SourceUrl { get; init; }
    public string Name { get; set; } = "New shop";
    public ShopStatus Status { get; set; } = ShopStatus.Writing;
    public ProductFacts? Product { get; set; }
    public StoreContent? Content { get; set; }
    public string? Error { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

/// <summary>What the product page says about the product, read as it is.</summary>
public sealed record ProductFacts(
    string Title,
    string? Description,
    decimal? Price,
    string? Currency,
    IReadOnlyList<string> Images,
    IReadOnlyList<string> Variants,
    string SourceUrl);

/// <summary>The words of the store, written around the product.</summary>
public sealed record StoreContent(
    string BrandName,
    string Tagline,
    string HomepageHeadline,
    string ProductDescription,
    IReadOnlyList<FaqItem> Faq,
    string About);

public sealed record FaqItem(string Question, string Answer);
