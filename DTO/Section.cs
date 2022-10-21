namespace vmProjectBFF.Models
{
    public class Section
    {
        public int SectionId { get; set; }
        public int CourseId { get; set; }
        public int SemesterId { get; set; }
        public int SectionNumber { get; set; }
        public int SectionCanvasId { get; set; }
        public string SectionName { get; set; }
        public string LibraryId { get; set; }
    }
}