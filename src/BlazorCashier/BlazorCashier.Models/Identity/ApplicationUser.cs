using Microsoft.AspNetCore.Identity;

namespace BlazorCashier.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsBlocked { get; set; }

        public string OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
