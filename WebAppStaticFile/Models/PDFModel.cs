namespace WebAppStaticFile.Models
{
    public class PDFModel
    {
        public string HtmlText { get; set; }
        public List<string> ImageReplate { get; set; } = new List<string>();
        public List<string> StudentId { get; set; } = new List<string>();
    }

    public class MergePDFModel
    {
        public List<string> Urls { get; set; } = new List<string>();
        public bool IsLog { get; set; } = false;
        public string? FileName { get; set; }
    }


}
