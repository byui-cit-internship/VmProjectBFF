using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("semester", Schema = "vmProject")]
    public class Semester
    {
        [Key]
        [Column("semester_id", Order = 1)]
        public int SectionId { get; set; }

        [Required]
        [Column("semester_year", Order = 2)]
        public int CourseId { get; set; }

        [Required]
        [Column("semester_term", TypeName = "varchar(20)", Order = 3)]
        public int SectionNumber { get; set; }

        [Required]
        [Column("start_date", TypeName = "datetime2(7)", Order = 4)]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date", TypeName = "datetime2(7)", Order = 5)]
        public DateTime EndDate { get; set; }
    }
}