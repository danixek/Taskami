using Microsoft.AspNetCore.Identity;

namespace Taskami.WebUI.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Sem se mohou dát další properties, např FirstName, etc.
    }
}
