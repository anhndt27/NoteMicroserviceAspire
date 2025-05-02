using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.ViewModel
{
	public class NoteSectionViewModel
	{
		public string Title { get; set; }
		public List<NoteSimpleResponseViewModel> Data { get; set; }
	}

	public class NotesViewModel
	{
		public NoteSectionViewModel SectionPrivate { get; set; }
		public NoteSectionViewModel SectionGroup { get; set; }
	}
}
