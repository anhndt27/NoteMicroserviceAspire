using System.ComponentModel.DataAnnotations.Schema;

namespace NoteMicroservice.Note.Domain.Entity;

public class BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual string Id { get; set; }
}