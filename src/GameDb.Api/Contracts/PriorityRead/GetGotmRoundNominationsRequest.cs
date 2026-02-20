using System.ComponentModel.DataAnnotations;

namespace GameDb.Api.Contracts.PriorityRead;

public sealed record GetGotmRoundNominationsRequest
{
    [Range(1, int.MaxValue)]
    public int RoundNumber { get; init; }
}
