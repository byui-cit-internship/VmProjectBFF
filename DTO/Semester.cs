namespace vmProjectBFF.Models
{
    public class Semester
    {
        public int SemesterId { get; set; }
        public int SemesterYear { get; set; }
        public string SemesterTerm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}