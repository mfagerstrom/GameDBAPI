using Microsoft.AspNetCore.Authorization;

namespace GameDb.Api.Authorization;

public static class AuthorizationExtensions
{
    public static void AddApiPolicies(this AuthorizationOptions options)
    {
        var sharedReadRoles = new[] { RoleNames.Member, RoleNames.Moderator, RoleNames.Admin, RoleNames.ServiceBot };

        options.AddPolicy(PolicyNames.PublicRead, policy =>
            policy.RequireAuthenticatedUser().RequireRole(sharedReadRoles));

        options.AddPolicy(PolicyNames.ProfileAndCollectionRead, policy =>
            policy.RequireAuthenticatedUser().RequireRole(sharedReadRoles));

        options.AddPolicy(PolicyNames.SuggestionRead, policy =>
            policy.RequireAuthenticatedUser().RequireRole(RoleNames.ServiceBot));

        options.AddPolicy(PolicyNames.AdminRead, policy =>
            policy.RequireAuthenticatedUser().RequireRole(RoleNames.Admin, RoleNames.ServiceBot));

        options.AddPolicy(PolicyNames.NominationWrite, policy =>
            policy.RequireAuthenticatedUser().RequireRole(sharedReadRoles));

        options.AddPolicy(PolicyNames.CollectionWrite, policy =>
            policy.RequireAuthenticatedUser().RequireRole(sharedReadRoles));
    }
}
