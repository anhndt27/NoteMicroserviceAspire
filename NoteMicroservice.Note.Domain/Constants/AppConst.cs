namespace NoteMicroservice.Note.Domain.Constants
{
    public static class AppConst
    {
        public const string ClaimName_Username = "name";
        public const string PolicyName_RequireScopes = "Requrie_Scopes";

        public static List<int> ValidsPageSizes { get; } = new List<int> { 5, 10, 25, 50, 100 };
    }
}
