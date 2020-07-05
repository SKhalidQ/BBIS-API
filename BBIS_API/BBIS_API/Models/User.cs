using System;
using System.ComponentModel.DataAnnotations;

namespace BBIS_API.Models
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Value for {0} has a max limit of {1} characters.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Value for {0} has a max limit of {1} characters.")]
        public string Password { get; set; }
    }

    public class GetUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
