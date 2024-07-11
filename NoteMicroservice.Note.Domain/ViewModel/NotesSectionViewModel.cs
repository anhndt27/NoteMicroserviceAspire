using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.ViewModel
{
	public class NotesSectionViewModel
	{
		public string Title { get; set; }
		public List<NoteSimpleResponseViewModel> Data { get; set; }
	}

	public class NotesViewModel
	{
		public NotesSectionViewModel SectionPrivate { get; set; }
		public NotesSectionViewModel SectionGroup { get; set; }
	}
}
