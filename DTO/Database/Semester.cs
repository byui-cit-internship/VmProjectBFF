using Newtonsoft.Json;
using VmProjectBFF.DTO.Canvas;

namespace VmProjectBFF.DTO.Database
{
    public class Semester
    {
        public int SemesterId { get; set; }
        public int SemesterYear { get; set; }
        public string SemesterTerm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EnrollmentTermCanvasId { get; set; }

        [JsonConstructor]
        public Semester(
            int semesterId,
            int semesterYear,
            string semesterTerm,
            DateTime startDate,
            DateTime endDate,
            int enrollmentTermCanvasId
        )
        {
            SemesterId = semesterId;
            SemesterYear = semesterYear;
            SemesterTerm = semesterTerm;
            StartDate = startDate;
            EndDate = endDate;
            EnrollmentTermCanvasId = enrollmentTermCanvasId;
        }

        public Semester(
            EnrollmentTerm term
        )
        {
            EnrollmentTermCanvasId = term.id;
            string[] splitTerm = term.name.Split(' ');
            SemesterYear = int.Parse(splitTerm[1]);
            SemesterTerm = splitTerm[0];
            StartDate = term.start_at.GetValueOrDefault();
            EndDate = term.end_at.GetValueOrDefault();
        }
    }
}