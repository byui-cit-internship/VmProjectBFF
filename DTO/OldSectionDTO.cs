namespace vmProjectBFF.DTO
{
    public class OldSectionDTO
    {
        public string sectionName;
        public int sectionId;
        public string semesterTerm;
        public int sectionNumber;
        public string fullName;

        public OldSectionDTO(string sectionName, int sectionId, string semesterTerm, int sectionNumber, string fullName)
        {
            this.sectionName = sectionName;
            this.sectionId = sectionId;
            this.semesterTerm = semesterTerm;
            this.sectionNumber = sectionNumber;
            this.fullName = fullName;
        }

    }
}
