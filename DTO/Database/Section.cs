namespace VmProjectBFF.DTO.Database
{
    public class Section
    {
        public int SectionId { get; set; }

        public int CourseId { get; set; }

        public int SemesterId { get; set; }

        public int FolderId { get; set; }

        public int ResourcePoolId { get; set; }

        public int SectionNumber { get; set; }

        public int SectionCanvasId { get; set; }

        public string SectionName { get; set; }

        public string LibraryVCenterId { get; set; }

        public string CourseCode { get; set;}
    }
}