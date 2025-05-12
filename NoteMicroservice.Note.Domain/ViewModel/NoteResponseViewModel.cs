using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Dto
{
	public class NoteSimpleResponseDto
	{
        public string Id { get; set; }
        public string Title { get; set; } = default!;
		public DateTimeOffset DateTime { get; set; }
        public string Category { get; set; }

    }

	public class NoteResponseDto : NoteSimpleResponseDto
	{
		public string NoteString { get; set; } = default!;
		public string UserId { get; set; } = default!;
	}

	public class NoteListFilter
	{
        public string Category { get; set; }
		public List<NoteDataItem> Data { get; set; }
	}

	public class NoteDataItem
	{
        public string Id { get; set; }
        public string Title { get; set; }
		public DateTimeOffset DateTime { get; set; }
		public string Category { get; set; }
	}
}
