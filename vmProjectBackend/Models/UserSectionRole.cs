using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("user_section_role", Schema = "vmProject")]
    public class UserSectionRole
    {
        [Key]
        [Column("user_section_role_id", Order = 1)]
        public int UserSectionRoleId { get; set; }

        [Required]
        [Column("user_id", Order = 2)]
        public int UserId { get; set; }

        [Required]
        [Column("section_id", Order = 3)]
        public int SectionId { get; set; }

        [Required]
        [Column("role_id", Order = 4)]
        public int RoleId { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}