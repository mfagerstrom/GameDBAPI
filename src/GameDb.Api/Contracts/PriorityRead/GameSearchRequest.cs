using System.ComponentModel.DataAnnotations;

namespace GameDb.Api.Contracts.PriorityRead;

public sealed record GameSearchRequest
{
    [Required(AllowEmptyStrings = false)]
    public required string Query { get; init; }

    public string? PlatformCode { get; init; }

    [Range(1, int.MaxValue)]
    public int Page { get; init; } = PriorityReadContractDefaults.DefaultPage;

    [Range(1, PriorityReadContractDefaults.MaxPageSize)]
    public int? PageSize { get; init; }

    [Range(1, PriorityReadContractDefaults.MaxPageSize)]
    public int? BotPageSize { get; init; }
}
