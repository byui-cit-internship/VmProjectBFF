namespace VmProjectBFF.DTO.Database
{
    public class Course
    {
        // Primary Key
        public int CourseId { get; set; }

        // Not Null
        public string CourseCode { get; set; }
        
        // Not Null
        // Links to ResourcePool
        public int ResourcePoolId { get; set; }
    }
}