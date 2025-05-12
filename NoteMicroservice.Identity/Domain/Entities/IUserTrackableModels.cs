namespace NoteMicroservice.Identity.Domain.Entities;

public interface IUserTrackableModels
{
    public string CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; }

    public string UpdatedByUserId { get; set; }
    public User UpdatedByUser { get; set; }
    
    public string DeletedByUserId { get; set; }
    public User DeletedByUser { get; set; }
}