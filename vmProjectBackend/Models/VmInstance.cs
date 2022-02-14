using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_instance", Schema = "vmProject")]
    public class VmInstance
    {
        [Key]
        [Column("vm_instance_id", Order = 1)]
        public int VmUserInstanceId { get; set; }

        [Required]
        [Column("vm_template_id", Order = 2)]
        public int VmTemplateId { get; set; }

        [Required]
        [Column("vm_instance_vcenter_id", TypeName = "varchar(50)", Order = 3)]
        public string VmUserInstanceVcenterId { get; set; }

        [Required]
        [Column("vm_instance_expire_date", TypeName = "datetime2(7)", Order = 4)]
        public DateTime vm_user_instance_expire_date { get; set; }


        [ForeignKey("VmTemplateId")]
        public VmTemplate VmTemplate { get; set; }
    }
}