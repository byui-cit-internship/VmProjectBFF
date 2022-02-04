using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("", Schema = "vmProject")]
    public class
    {
        [Key]
    [Column("_id", Order = 1)]
    public int Id { get; set; }

    [Required]
    [Column("role_name", TypeName = "varchar(20)", Order = 2)]
    public string RoleName { get; set; }


    [ForeignKey("Id")]
    public  { get; set; }
}
}