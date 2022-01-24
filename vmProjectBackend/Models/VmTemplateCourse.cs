using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_template_course", Schema = "vmProject")]
    public class VmTemplateCourse
    {
        [Key]
        [Column("vm_template_course_id", Order = 1)]
        public int VmTemplateCourseId { get; set; }

        [Required]
        [Column("vm_template_id", Order = 2)]
        public int VmTemplateId { get; set; }

        [Required]
        [Column("vm_course_id", Order = 3)]
        public int VmCourseId { get; set; }


        [ForeignKey("VmTemplateId")]
        public VmTemplate VmTemplate { get; set; }

        [ForeignKey("VmCourseId")]
        public Course Course { get; set; }
    }
}