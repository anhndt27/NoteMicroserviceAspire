using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.Dto
{
	public class NoteSectionDto
	{
		public string Title { get; set; }
		public List<NoteSimpleResponseDto> Data { get; set; }
	}

	public class NotesDto
	{
		public NoteSectionDto SectionPrivate { get; set; }
		public NoteSectionDto SectionGroup { get; set; }
	}
}
