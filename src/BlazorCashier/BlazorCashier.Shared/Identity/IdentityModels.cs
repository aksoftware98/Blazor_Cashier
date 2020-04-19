using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorCashier.Shared.Identity
{
    public class RegisterRequest
    {

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string CountryId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string City { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 5)]
        public string Address1 { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 5)]
        public string Address2 { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        public string RoleId { get; set; }
    }

    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }

    public class LoginRequest
    {

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }

    }
}
