using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("section", Schema = "vmProject")]
    public class Section
    {
        [Key]
        [Column("section_id", Order = 1)]
        public int SectionId { get; set; }

        [Required]
        [Column("course_id", Order = 2)]
        public int CourseId { get; set; }

        [Required]
        [Column("semester_id", Order = 3)]
        public int SemesterId { get; set; }

        [Required]
        [Column("section_number", Order = 4)]
        public int SectionNumber { get; set; }

        [Required]
        [Column("section_canvas_id", Order = 5)]
        public int SectionCanvasId { get; set; }



        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [ForeignKey("SemesterId")]
        public Semester Semester { get; set; }
    }
}