using GameDb.Api.Authorization;
using GameDb.Api.Contracts.PriorityRead;
using GameDb.Api.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameDb.Api.Controllers;

[ApiController]
[Route("api/v1/games")]
[Authorize(Policy = PolicyNames.PublicRead)]
public sealed class GamesController : ControllerBase
{
    private readonly IGameSearchQueryService _gameSearchQueryService;

    public GamesController(IGameSearchQueryService gameSearchQueryService)
    {
        _gameSearchQueryService = gameSearchQueryService;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(GameSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GameSearchResponse>> SearchAsync([FromQuery] GameSearchRequest request, CancellationToken cancellationToken)
    {
        var response = await _gameSearchQueryService.SearchAsync(request, cancellationToken);
        return Ok(response);
    }
}
