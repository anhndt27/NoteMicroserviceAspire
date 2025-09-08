namespace NoteMicroservice.Note.Domain.ViewModel;

public class NoteRequestDto
{
	public string Title { get; set; }
	public string NoteString { get; set; }
	public string GroupId { get; set; }
}

public class NoteReactDto
{
	public string Title { get; set; } 
	public string NoteString { get; set; }
}

public class UpdateCategoryRequest
{
	public string Category { get; set; }
	public string GroupId { get; set; }
}