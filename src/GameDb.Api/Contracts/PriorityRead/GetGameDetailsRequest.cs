using System.ComponentModel.DataAnnotations;

namespace GameDb.Api.Contracts.PriorityRead;

public sealed record GetGameDetailsRequest
{
    [Range(1, long.MaxValue)]
    public long GameId { get; init; }
}
