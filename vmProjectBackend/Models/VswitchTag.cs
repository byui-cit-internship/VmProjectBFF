using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vswitch_tag", Schema = "vmProject")]
    public class VswitchTag
    {
        [Key]
        [Column("vswitch_tag_id", Order = 1)]
        public int VswitchTagId { get; set; }

        [Required]
        [Column("tag_id", Order = 2)]
        public int TagId { get; set; }

        [Required]
        [Column("vswitch_id", Order = 3)]
        public int VswitchId { get; set; }


        [ForeignKey("TagId")]
        public Tag Tag { get; set; }

        [ForeignKey("VswitchId")]
        public Vswitch Vswitch { get; set; }
    }
}