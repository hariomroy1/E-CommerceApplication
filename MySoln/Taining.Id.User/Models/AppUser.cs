using Microsoft.AspNetCore.Identity;

namespace Taining.Id.User.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

    }
}
