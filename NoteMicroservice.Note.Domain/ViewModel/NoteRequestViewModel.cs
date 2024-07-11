using NoteMicroservice.Note.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.ViewModel
{
	public class NoteRequestViewModel
	{
		public string Title { get; set; } = default!;
		public string NoteString { get; set; } = default!;
		public string UserId { get; set; } = default!;
		public int? GroupId { get; set; }
		public StatusAccess StatusAccess { get; set; }
	}

	public class NoteReactViewModel
	{
		public string Title { get; set; } = default!;
		public string NoteString { get; set; } = default!;
		public StatusAccess StatusAccess { get; set; }
	}

	public class UpdateCategoryRequest
	{
		public string Category { get; set; }
		public int GroupId { get; set; }
	}
}
