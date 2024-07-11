namespace NoteMicroservice.Note.Domain.Entity;

public class NoteContent {
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string NoteString { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public int? GroupId { get; set; }
    public StatusAccess StatusAccess { get; set; }
    public DateTimeOffset DateTime { get; set; }
}

public enum StatusAccess {
    publicEdit,
    readOnly,
}