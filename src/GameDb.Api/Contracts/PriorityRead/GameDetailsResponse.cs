namespace GameDb.Api.Contracts.PriorityRead;

public sealed record GameDetailsResponse
{
    public long GameId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public long? IgdbId { get; init; }
    public string? IgdbUrl { get; init; }
    public decimal? TotalRating { get; init; }
    public DateOnly? InitialReleaseDate { get; init; }
    public IReadOnlyList<GameDetailsPlatformResponse> Platforms { get; init; } = [];
    public IReadOnlyList<GameDetailsReleaseResponse> Releases { get; init; } = [];
}

public sealed record GameDetailsPlatformResponse
{
    public required string PlatformCode { get; init; }
    public required string PlatformName { get; init; }
}

public sealed record GameDetailsReleaseResponse
{
    public long ReleaseId { get; init; }
    public required string PlatformCode { get; init; }
    public required string RegionCode { get; init; }
    public DateOnly? ReleaseDate { get; init; }
    public required string Format { get; init; }
}
