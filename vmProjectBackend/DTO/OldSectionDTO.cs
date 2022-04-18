namespace vmProjectBackend.DTO
{
    public class OldSectionDTO
    {
        private string courseName;
        private int sectionId;
        private string semesterTerm;
        private int sectionNumber;
        private string fullName;

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
