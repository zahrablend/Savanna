using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Savanna.Web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nickname can't be empty")]
    public string NickName { get; set; }

    [Required(ErrorMessage = "Email can't be empty")]
    [EmailAddress(ErrorMessage = "Email should be in a valid format")]
    [Remote(action: "IsUserAlreadyRegistered", controller: "Account", ErrorMessage ="Email is already in use")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password can't be empty")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password can't be empty")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage ="Password entries do not match")]
    public string ConfirmPassword { get; set; }
}
