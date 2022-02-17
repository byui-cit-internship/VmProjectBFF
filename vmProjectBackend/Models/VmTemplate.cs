using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_template", Schema = "vmProject")]
    public class VmTemplate
    {
        [Key]
        [Column("vm_template_id", Order = 1)]
        public int VmTemplateId { get; set; }

        [Required]
        [Column("vm_template_vcenter_id", TypeName = "varchar(50)", Order = 2)]
        public string VmTemplateVcenterId { get; set; }

        [Required]
        [Column("vm_template_name", TypeName = "varchar(50)", Order = 3)]
        public string VmTemplateName { get; set; }

        [Required]
        [Column("vm_template_access_date", TypeName = "datetime2(7)", Order = 4)]
        public DateTime VmTemplateAccessDate { get; set; }
    }
}