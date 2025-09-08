using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Dtos.BaseDtos;

public class EntityDetailDto
{
    public string Id { get; set; }

    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }

    public string CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; }

    public string UpdatedByUserId { get; set; }
    public string UpdatedByUserName { get; set; }

    public virtual void MapBaseProperties<TModel>(TModel entity) where TModel : BaseModel
    {
        Id = entity.Id;

        if (entity is IUserTrackableModels)
        {
            CreatedByUserId = ((IUserTrackableModels)entity).CreatedByUserId;
            CreatedByUserName = ((IUserTrackableModels)entity).CreatedByUserId;

            UpdatedByUserId = ((IUserTrackableModels)entity).UpdatedByUserId;
            UpdatedByUserName = ((IUserTrackableModels)entity).UpdatedByUserId;
        }

        if (entity is ITimeTrackableModel)
        {
            CreatedTimeUtc = ((ITimeTrackableModel)entity).CreatedTimeUtc;
            UpdatedTimeUtc = ((ITimeTrackableModel)entity).CreatedTimeUtc;
        }
    }
}