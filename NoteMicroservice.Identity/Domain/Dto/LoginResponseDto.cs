namespace NoteMicroservice.Identity.Domain.Dto
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<string> GroupIds { get; set; }
        public List<string> GroupNames { get; set; }
    }
}
