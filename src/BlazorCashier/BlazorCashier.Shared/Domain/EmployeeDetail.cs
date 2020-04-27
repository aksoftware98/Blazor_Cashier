using BlazorCashier.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorCashier.Shared.Domain
{
    public class EmployeeDetail
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string StreetAddress { get; set; }
        public string Description { get; set; }
        public DateTime Birthdate { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsBlocked { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public string RoleId { get; set; }
        public IFormFile ProfileImage { get; set; }

        public string OrganizationId { get; set; }

        public EmployeeDetail()
        {
            
        }

        public EmployeeDetail(Employee employee)
        {
            Id = employee.Id;
            UserId = employee.UserId;
            FirstName = employee.User.FirstName;
            LastName = employee.User.LastName;
            Email = employee.User.Email;
            ProfilePicture = employee.User.ProfilePicture;
            IsBlocked = employee.User.IsBlocked;
            City = employee.City;
            Address = employee.Address;
            Description = employee.Description;
            Birthdate = employee.BirthDate;
        }
    }
}
