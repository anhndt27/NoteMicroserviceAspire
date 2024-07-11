namespace NoteMicroservice.Identity.Domain.ViewModel
{
	public class GroupRequestViewModel
	{
        public string UserId { get; set; }
        public string GroupName { get; set; }
    }

    public class ReactGroupViewModel
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }
    }

    public class JoinGroupViewModel
    {
        public string UserId { get; set;}
        public string GroupCode { get; set; }
    }
}
