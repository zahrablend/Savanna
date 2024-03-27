using System.ComponentModel.DataAnnotations;

namespace Savanna.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email can't be empty")]
        [EmailAddress(ErrorMessage = "Email should be in a valid format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
