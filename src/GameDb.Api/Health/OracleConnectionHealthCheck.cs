using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GameDb.Api.Health;

public sealed class OracleConnectionHealthCheck : IHealthCheck
{
    public const string Name = "oracle-connection";
    private const string OracleConnectionStringName = "Oracle";

    private readonly IConfiguration _configuration;

    public OracleConnectionHealthCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString(OracleConnectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(
                $"ConnectionStrings:{OracleConnectionStringName} is not configured."));
        }

        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
