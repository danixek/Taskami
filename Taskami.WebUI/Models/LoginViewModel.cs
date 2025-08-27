using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace Taskami.WebUI.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Display(Name = "Zapamatuj si mě")]
        public bool RememberMe { get; set; }
    }
}