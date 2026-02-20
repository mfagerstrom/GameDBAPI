using GameDb.Api.Contracts.PriorityRead;

namespace GameDb.Api.Games;

public interface IGameSearchQueryService
{
    Task<GameSearchResponse> SearchAsync(GameSearchRequest request, CancellationToken cancellationToken);
}
