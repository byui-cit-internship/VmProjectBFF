using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vlan_vswitch", Schema = "vmProject")]
    public class VlanVswitch
    {
        [Key]
        [Column("vlan_vswitch_id", Order = 1)]
        public int VlanVswitchId { get; set; }

        [Required]
        [Column("vlan_id", Order = 2)]
        public int VlanId { get; set; }

        [Required]
        [Column("vswitch_id", Order = 3)]
        public int VswitchId { get; set; }


        [ForeignKey("VlanId")]
        public Vlan Vlan { get; set; }

        [ForeignKey("VswitchId")]
        public Vswitch Vswitch { get; set; }
    }
}