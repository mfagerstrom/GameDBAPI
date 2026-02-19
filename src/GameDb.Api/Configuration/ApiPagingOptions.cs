using System.ComponentModel.DataAnnotations;

namespace GameDb.Api.Configuration;

public sealed class ApiPagingOptions
{
    public const string SectionName = "ApiPaging";

    [Range(1, int.MaxValue)]
    public int DefaultPage { get; init; } = 1;

    [Range(1, int.MaxValue)]
    public int DefaultPageSize { get; init; } = 25;

    [Range(1, int.MaxValue)]
    public int DefaultBotPageSize { get; init; } = 10;

    [Range(1, int.MaxValue)]
    public int MaxPageSize { get; init; } = 100;
}
