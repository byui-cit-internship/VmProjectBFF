


namespace vmProjectBFF.Models
{
    public class Course
    {
        // Primary Key
        public int CourseId { get; set; }

        // Not Null
        public string CourseCode { get; set; }

        // Not Null
        public string CourseName { get; set; }

        // Not Null
        // Links to ResourceGroup
        public int ResourceGroupId { get; set; }
    }
}