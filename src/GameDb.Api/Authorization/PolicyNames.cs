namespace GameDb.Api.Authorization;

public static class PolicyNames
{
    public const string PublicRead = "PublicRead";
    public const string ProfileAndCollectionRead = "ProfileAndCollectionRead";
    public const string SuggestionRead = "SuggestionRead";
    public const string AdminRead = "AdminRead";
    public const string NominationWrite = "NominationWrite";
    public const string CollectionWrite = "CollectionWrite";
}
