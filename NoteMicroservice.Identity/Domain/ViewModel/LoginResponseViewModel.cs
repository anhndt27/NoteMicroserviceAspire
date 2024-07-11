namespace NoteMicroservice.Identity.Domain.ViewModel
{
    public class LoginResponseViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int? GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
    }
}
