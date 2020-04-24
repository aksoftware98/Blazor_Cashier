namespace BlazorCashier.Shared.Identity
{
    public class CreateApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OrganizationId { get; set; }
        public string RoleId { get; set; }
    }
}
