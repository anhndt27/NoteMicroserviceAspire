using NoteMicroservice.Note.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.Dto
{
	public class NoteRequestDto
	{
		public string Title { get; set; } = default!;
		public string NoteString { get; set; } = default!;
		public string UserId { get; set; } = default!;
		public string GroupId { get; set; }
	}

	public class NoteReactDto
	{
		public string Title { get; set; } = default!;
		public string NoteString { get; set; } = default!;
	}

	public class UpdateCategoryRequest
	{
		public string Category { get; set; }
		public string GroupId { get; set; }
	}
}
