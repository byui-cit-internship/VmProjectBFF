using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.DTO
{
    public class CourseCreate
    {
        public string sectionName { get; set; }
        public string courseCode { get; set; }
        public string canvas_token { get; set; }
        public int section_num { get; set; }
        public Semester semester { get; set; }
        public string libraryId { get; set; }
        public string folder { get; set; }
        public List<string> templateVm { get; set; }
        public string vmTemplateName {get; set;}
        public string resource_pool { get; set; }
        public string resourcePoolName { get; set; }
        public string userId { get; set; }
        public string canvasCourseId { get; set; }
    }
}
