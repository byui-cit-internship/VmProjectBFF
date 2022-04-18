namespace vmProjectBackend.DTO
{
    public class SectionDTO
    {
        public int? SectionId { get; set; }

        public int? CourseId { get; set; }

        public int? SemesterId { get; set; }

        public int? FolderId { get; set; }

        public int? ResourceGroupId { get; set; }

        public int? SectionNumber { get; set; }

        public int? SectionCanvasId { get; set; }

        public SectionDTO(int? courseId, int? semesterId, int? folderId, int? resourceGroupId, int? sectionNumber, int? sectionCanvasId)
        {
            CourseId = courseId;
            SemesterId = semesterId;   
            FolderId = folderId;
            ResourceGroupId = resourceGroupId;
            SectionNumber = sectionNumber;
            SectionCanvasId = sectionCanvasId;
        }

        public SectionDTO()
        {

        }
    }
}
