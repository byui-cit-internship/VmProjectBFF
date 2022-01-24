using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_user_instance", Schema = "vmProject")]
    public class VmUserInstance
    {
        [Key]
        [Column("vm_user_instance_id", Order = 1)]
        public int VmUserInstanceId { get; set; }
        
        [Required]
        [Column("user_section_role_id", Order = 2)]
        public int UserSectionRoleId { get; set; }

        [Required]
        [Column("vm_template_id", Order = 3)]
        public int VmTemplateId { get; set; }

        [Required]
        [Column("vm_user_instance_vcenter_id", TypeName = "varchar(50)", Order = 4)]
        public string VmUserInstanceVcenterId { get; set; }

        [Required]
        [Column("vm_user_instance_expire_date", TypeName = "datetime2(7)", Order = 5)]
        public DateTime vm_user_instance_expire_date { get; set; }


        [ForeignKey("UserSectionRoleId")]
        public UserSectionRole UserSectionRole { get; set; }

        [ForeignKey("VmTemplateId")]
        public VmTemplate VmTemplate { get; set; }
    }
}