using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vswitch", Schema = "vmProject")]
    public class Vswitch
    {
        [Key]
        [Column("vswitch_id", Order = 1)]
        public int VswitchId { get; set; }

        [Required]
        [Column("vswitch_name", TypeName = "varchar(45)", Order = 2)]
        public string VswitchName { get; set; }

        [Required]
        [Column("vswitch_description", TypeName = "varchar(45)", Order = 2)]
        public string VswitchDescription { get; set; }
    }
}