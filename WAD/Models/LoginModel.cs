using System.ComponentModel.DataAnnotations;

namespace WAD.Models
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}