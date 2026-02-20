namespace GameDb.Api.Contracts.PriorityRead;

public sealed record GotmRoundNominationsResponse
{
    public int RoundNumber { get; init; }
    public IReadOnlyList<GotmRoundNominationItemResponse> Items { get; init; } = [];
}

public sealed record GotmRoundNominationItemResponse
{
    public int NominationId { get; init; }
    public required string UserId { get; init; }
    public long GameId { get; init; }
    public required string GameTitle { get; init; }
    public string? Reason { get; init; }
    public DateTimeOffset NominatedAt { get; init; }
}
