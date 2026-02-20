namespace GameDb.Api.Contracts.PriorityRead;

public sealed record PlatformListResponse
{
    public IReadOnlyList<PlatformItemResponse> Items { get; init; } = [];
}

public sealed record PlatformItemResponse
{
    public int PlatformId { get; init; }
    public required string PlatformCode { get; init; }
    public required string PlatformName { get; init; }
}
