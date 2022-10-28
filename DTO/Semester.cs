using vmProjectBFF.DTO;
using Newtonsoft.Json;

namespace vmProjectBFF.Models
{
    public class Semester
    {
        public int SemesterId { get; set; }
        public int SemesterYear { get; set; }
        public string SemesterTerm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EnrollmentTermId { get; set; }

        [JsonConstructor]
        public Semester(
            int semesterId,
            int semesterYear,
            string semesterTerm,
            DateTime startDate,
            DateTime endDate,
            int enrollmentTermId
        )
        {
            SemesterId = semesterId;
            SemesterYear = semesterYear;
            SemesterTerm = semesterTerm;
            StartDate = startDate;
            EndDate = endDate;
            EnrollmentTermId = enrollmentTermId;
        }

        public Semester(
            Test1 term
        )
        {
            EnrollmentTermId = term.id;
            string[] splitTerm = term.name.Split(' ');
            SemesterYear = int.Parse(splitTerm[1]);
            SemesterTerm = splitTerm[0];
            StartDate = term.start_at.GetValueOrDefault();
            EndDate = term.end_at.GetValueOrDefault();
        }
    }
}