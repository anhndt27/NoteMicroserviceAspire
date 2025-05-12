namespace NoteMicroservice.Identity.Domain.Entities
{
    public class Group : BaseModel, ISoftDeletedModel, IUserTrackableModels, ITimeTrackableModel
    {
        public string Name { get; set; }
        
        public List<UserGroups> UserGroups { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTimeUtc { get; set; }
        public string DeletedByUserId { get; set; }
        public User DeletedByUser { get; set; }
        public string CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }
        public User UpdatedByUser { get; set; }
        public DateTimeOffset CreatedTimeUtc { get; set; }
        public DateTimeOffset? UpdatedTimeUtc { get; set; }
    }
}
