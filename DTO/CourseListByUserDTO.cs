namespace VmProjectBFF.DTO
{
    public class CourseListByUserDTO
    {
        public int canvasSectionId { get; set; }
        public int sectionId { get; set; }
        public string sectionName { get; set; }
        public int sectionRoleId { get; set; }
        public string studentFullName { get; set; }
        public string vcenterTemplateId { get; set; }
        public string LibraryVCenterId {get; set; } 
        public CourseListByUserDTO(int canvasSectionId, int sectionId, string sectionName, int sectionRoleId, string studentFullName, string vcenterTemplateId, string LibraryVCenterId)
        {
            this.canvasSectionId = canvasSectionId;
            this.sectionId = sectionId;
            this.sectionName = sectionName;
            this.sectionRoleId = sectionRoleId;
            this.studentFullName = studentFullName;
            this.vcenterTemplateId = vcenterTemplateId;
            this.LibraryVCenterId = LibraryVCenterId;
        }
    }
}