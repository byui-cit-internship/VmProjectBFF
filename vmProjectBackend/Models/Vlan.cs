using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vlan", Schema = "vmProject")]
    public class Vlan
    {
        [Key]
        [Column("vlan_id", Order = 1)]
        public int VlanId { get; set; }

        [Required]
        [Column("vlan_number", Order = 2)]
        public int VlanNumber { get; set; }

        [Column("vlan_description", TypeName = "varchar(100)", Order = 3)]
        public string VlanDescription { get; set; }
    }
}