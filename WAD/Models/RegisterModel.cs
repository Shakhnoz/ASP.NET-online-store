using System.ComponentModel.DataAnnotations;

namespace WAD.Models
{
    public class RegisterModel
    {
        [Display(Name = "First name")]
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Please provide at least 2 characters")]
        public string Firstname { get; set; }

        [Display(Name = "Last name")]
        [Required]
        [StringLength(20)]
        public string Lastname { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Please provide at least 4 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Please provide at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}