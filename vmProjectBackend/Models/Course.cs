using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("course", Schema = "vmProject")]
    public class Course
    {
        // Primary Key
        [Key]
        [Column("course_id", Order = 1)]
        public int CourseId { get; set; }
        
        [Required]
        [Column("course_code", TypeName = "varchar(15)", Order = 2)]
        public string CourseCode { get; set; }

        [Required]
        [Column("course_name", TypeName = "varchar(75)", Order = 3)]
        public string CourseName { get; set; }

        public int Section { get; set; }

        public string ContentLibrary { get; set; }

        public string TemplateVm { get; set; }

        public string Semester { get; set; }

        public string Description { get; set; }

        public string Folder { get; set; }

        public string Resource_pool { get; set; }

    }
}