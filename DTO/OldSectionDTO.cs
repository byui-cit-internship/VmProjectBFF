namespace vmProjectBFF.DTO
{
    public class OldSectionDTO
    {
        public string courseName;
        public int sectionId;
        public string semesterTerm;
        public int sectionNumber;
        public string fullName;

        public OldSectionDTO(string courseName, int sectionId, string semesterTerm, int sectionNumber, string fullName)
        {
            this.courseName = courseName;
            this.sectionId = sectionId;
            this.semesterTerm = semesterTerm;
            this.sectionNumber = sectionNumber;
            this.fullName = fullName;
        }

    }
}
