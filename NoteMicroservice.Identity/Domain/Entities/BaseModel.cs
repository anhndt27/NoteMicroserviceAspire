using System.ComponentModel.DataAnnotations.Schema;

namespace NoteMicroservice.Identity.Domain.Entities;

public class BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual string Id { get; set; }
}