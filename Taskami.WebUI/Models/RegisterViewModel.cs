using System.ComponentModel.DataAnnotations;
namespace Taskami.WebUI.Models
{
    public class RegisterViewModel
    {
        [EmailAddress]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují")]
        public required string ConfirmPassword { get; set; }
    }
}