using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("access_token", Schema = "vmProject")]
    public class AccessToken
    {
        [Key]
        [Column("access_token_id", Order = 1)]
        public int AccessTokenId { get; set; }

        [Required]
        [Column("access_token_value", TypeName = "varchar(200)", Order = 2)]
        public string AccessTokenValue { get; set; }

        [Required]
        [Column("expire_date", TypeName = "datetime2(7)", Order = 3)]
        public DateTime ExpireDate { get; set; }

        [Required]
        [Column("user_id", Order = 4)]
        public int UserId { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}