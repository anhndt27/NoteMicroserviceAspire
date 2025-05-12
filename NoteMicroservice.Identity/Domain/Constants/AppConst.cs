namespace NoteMicroservice.Identity.Domain.Constants
{
    public static class AppConst
    {
        public const string ClaimName_Username = "name";

        public static List<int> ValidsPageSizes { get; } = new List<int> { 5, 10, 25, 50, 100 };
    }
}
