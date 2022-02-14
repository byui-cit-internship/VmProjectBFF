using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_instance_vswitch", Schema = "vmProject")]
    public class VmInstanceVswitch
    {
        [Key]
        [Column("vm_instance_vswitch_id", Order = 1)]
        public int VmInstanceVswitchId { get; set; }

        [Required]
        [Column("vm_instance_id", Order = 2)]
        public int VmInstanceId { get; set; }

        [Required]
        [Column("vswitch_id", Order = 3)]
        public int VswitchId { get; set; }


        [ForeignKey("VmInstanceId")]
        public VmInstance VmInstance { get; set; }

        [ForeignKey("VswitchId")]
        public Vswitch Vswitch { get; set; }
    }
}