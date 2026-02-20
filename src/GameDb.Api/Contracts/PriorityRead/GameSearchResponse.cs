namespace GameDb.Api.Contracts.PriorityRead;

public sealed record GameSearchResponse
{
    public IReadOnlyList<GameSearchItemResponse> Items { get; init; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
}

public sealed record GameSearchItemResponse
{
    public long GameId { get; init; }
    public required string Title { get; init; }
    public long? IgdbId { get; init; }
    public bool ThumbnailApproved { get; init; }
    public DateOnly? InitialReleaseDate { get; init; }
    public IReadOnlyList<string> Platforms { get; init; } = [];
}
