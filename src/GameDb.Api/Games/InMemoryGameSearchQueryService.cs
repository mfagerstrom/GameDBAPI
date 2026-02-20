using GameDb.Api.Configuration;
using GameDb.Api.Contracts.PriorityRead;
using Microsoft.Extensions.Options;

namespace GameDb.Api.Games;

public sealed class InMemoryGameSearchQueryService : IGameSearchQueryService
{
    private static readonly IReadOnlyList<GameSearchItemResponse> SeedGames =
    [
        new GameSearchItemResponse
        {
            GameId = 123,
            Title = "Chrono Trigger",
            IgdbId = 358,
            ThumbnailApproved = true,
            InitialReleaseDate = new DateOnly(1995, 3, 11),
            Platforms = ["SNES", "PC"]
        },
        new GameSearchItemResponse
        {
            GameId = 124,
            Title = "Chrono Cross",
            IgdbId = 1635,
            ThumbnailApproved = true,
            InitialReleaseDate = new DateOnly(1999, 11, 18),
            Platforms = ["PS1", "PSN"]
        },
        new GameSearchItemResponse
        {
            GameId = 200,
            Title = "Final Fantasy VI",
            IgdbId = 125,
            ThumbnailApproved = true,
            InitialReleaseDate = new DateOnly(1994, 4, 2),
            Platforms = ["SNES", "GBA", "PC"]
        }
    ];

    private readonly ApiPagingOptions _pagingOptions;

    public InMemoryGameSearchQueryService(IOptions<ApiPagingOptions> pagingOptions)
    {
        _pagingOptions = pagingOptions.Value;
    }

    public Task<GameSearchResponse> SearchAsync(GameSearchRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var effectivePage = request.Page <= 0 ? _pagingOptions.DefaultPage : request.Page;
        var effectivePageSize = ResolvePageSize(request);

        var filtered = SeedGames
            .Where(game => game.Title.Contains(request.Query, StringComparison.OrdinalIgnoreCase))
            .Where(game => string.IsNullOrWhiteSpace(request.PlatformCode) ||
                           game.Platforms.Any(platform => platform.Equals(request.PlatformCode, StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        var items = filtered
            .Skip((effectivePage - 1) * effectivePageSize)
            .Take(effectivePageSize)
            .ToArray();

        var response = new GameSearchResponse
        {
            Items = items,
            Page = effectivePage,
            PageSize = effectivePageSize,
            TotalCount = filtered.Length
        };

        return Task.FromResult(response);
    }

    private int ResolvePageSize(GameSearchRequest request)
    {
        var selectedPageSize = request.BotPageSize
                               ?? request.PageSize
                               ?? _pagingOptions.DefaultPageSize;

        return Math.Clamp(selectedPageSize, 1, _pagingOptions.MaxPageSize);
    }
}
