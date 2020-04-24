using Microsoft.AspNetCore.Identity;

namespace BlazorCashier.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        #region Constructors

        public ApplicationUser()
        {

        }

        #endregion

        #region Public Properties

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsBlocked { get; set; }
        public string OrganizationId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Organization Organization { get; set; }

        #endregion
    }
}
