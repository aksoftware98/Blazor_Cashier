using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Services.Account;
using BlazorCashier.Services.Common;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using BlazorCashier.Shared.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        #region Private Members

        private readonly IUserService _userService;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Organization> _orgRepository;
        private readonly IWebHostEnvironmentProvider _envProvider;
        private readonly ISettings _settings;

        #endregion

        #region Constructors

        public EmployeeService(
            IUserService userService,
            IRepository<Employee> employeeRepository,
            IRepository<Organization> orgRepository,
            IWebHostEnvironmentProvider envProvider,
            ISettings settings)
        {
            _userService = userService;
            _employeeRepository = employeeRepository;
            _orgRepository = orgRepository;
            _envProvider = envProvider;
            _settings = settings;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<EmployeeDetail>> CreateEmployeeAsync(EmployeeDetail employeeDetail, string currentUserId)
        {
            if (employeeDetail is null)
                throw new ArgumentNullException(nameof(employeeDetail));

            var org = await _orgRepository.GetByIdAsync(employeeDetail.OrganizationId);

            if (org is null)
                return new EntityApiResponse<EmployeeDetail>(error: "Organization does not exist");

            if (employeeDetail.Password != employeeDetail.ConfirmPassword)
                return new EntityApiResponse<EmployeeDetail>(error: "Passwords do not match");

            var uploadedFile = !(employeeDetail.ProfileImage is null);

            var usersImagesFolderPath = Path.Combine(_envProvider.WebRootPath, "Images", "Users");

            var profileImageFileName = "default.png";

            if (uploadedFile)
            {
                var extension = Path.GetExtension(employeeDetail.ProfileImage.FileName);

                if (!_settings.SupportedExtensions.Contains(extension))
                    return new EntityApiResponse<EmployeeDetail>(error: "File extension is not supported");

                if (employeeDetail.ProfileImage.Length > _settings.MaxImageSize)
                    return new EntityApiResponse<EmployeeDetail>(error: "File size is too large");

                profileImageFileName = $"{Path.GetFileNameWithoutExtension(employeeDetail.ProfileImage.FileName)}_{Guid.NewGuid()}{extension}";
            }

            var createUserResponse = await _userService.CreateUserAsync(new CreateApplicationUser
            {
                FirstName = employeeDetail.FirstName?.Trim(),
                LastName = employeeDetail.LastName?.Trim(),
                Email = employeeDetail.Email,
                Password = employeeDetail.Password,
                ProfilePicture = Path.Combine(usersImagesFolderPath, profileImageFileName).Replace("\\","/"),
                RoleId = employeeDetail.RoleId,
                OrganizationId = org.Id
            });

            if (!createUserResponse.IsSuccess)
                return new EntityApiResponse<EmployeeDetail>(error: createUserResponse.Error);

            var newEmployee = new Employee
            {
                Address = employeeDetail.Address?.Trim(),
                StreetAddress = employeeDetail.StreetAddress?.Trim(),
                City = employeeDetail.City?.Trim(),
                Description = employeeDetail.Description?.Trim(),
                BirthDate = employeeDetail.Birthdate.ToUniversalTime(),
                UserId = createUserResponse.Entity.Id,
                CreatedById = currentUserId,
                ModifiedById = currentUserId,
                OrganizationId = org.Id
            };

            await _employeeRepository.InsertAsync(newEmployee);
            newEmployee.User = createUserResponse.Entity;

            if (uploadedFile)
                using (Stream stream = new FileStream(createUserResponse.Entity.ProfilePicture, FileMode.Create))
                    await employeeDetail.ProfileImage.CopyToAsync(stream);

            return new EntityApiResponse<EmployeeDetail>(entity: new EmployeeDetail(newEmployee));
        }

        public async Task<ApiResponse> DeleteEmployeeAsync(string employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee is null)
                return new ApiResponse("Employee does not exist");

            var imageFileName = Path.GetFileName(employee.User.ProfilePicture);

            // Delete the employee
            await _employeeRepository.DeleteAsync(employee);

            // Delete the related application user
            var response = await _userService.DeleteUserAsync(employee.User);

            // Delete profile image if it is not the default one
            if (!string.Equals(imageFileName, "default.png"))
            {
                var imagePhysicalPath = Path.Combine(_envProvider.WebRootPath, "Images", "Users", imageFileName);
                File.Delete(imagePhysicalPath);
            }

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<EmployeeDetail>> GetEmployeeDetailsAsync(string employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee is null)
                return new EntityApiResponse<EmployeeDetail>(error: "Employee does not exist");

            return new EntityApiResponse<EmployeeDetail>(entity: new EmployeeDetail(employee));
        }

        public async Task<EntitiesApiResponse<EmployeeDetail>> GetEmployeesForOrganizationAsync(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<EmployeeDetail>(error: "Organization does not exist");

            var employees = await _employeeRepository.Table
                .Where(e => e.OrganizationId == org.Id).ToListAsync();

            var employeesDetails = employees.Select(e => new EmployeeDetail(e));

            return new EntitiesApiResponse<EmployeeDetail>(entities: employeesDetails);
        }

        public async Task<EntityApiResponse<EmployeeDetail>> UpdateEmployeeAsync(EmployeeDetail employeeDetail, string currentUserId)
        {
            if (employeeDetail is null)
                throw new ArgumentNullException(nameof(employeeDetail));

            var employee = await _employeeRepository.GetByIdAsync(employeeDetail.Id);

            if (employee is null)
                return new EntityApiResponse<EmployeeDetail>(error: "Employee does not exist");

            var uploadedFile = !(employeeDetail.ProfileImage is null);

            var profileImageFileName = Path.GetFileName(employee.User.ProfilePicture);

            // Keep a copy in case of updating to delete the old image
            var oldProfilePictureFileName = profileImageFileName;

            var usersImagesFolderPath = Path.Combine(_envProvider.WebRootPath, "Images", "Users");

            if (uploadedFile)
            {
                var extension = Path.GetExtension(employeeDetail.ProfileImage.FileName);

                if (!_settings.SupportedExtensions.Contains(extension))
                    return new EntityApiResponse<EmployeeDetail>(error: "File extension is not supported");

                if (employeeDetail.ProfileImage.Length > _settings.MaxImageSize)
                    return new EntityApiResponse<EmployeeDetail>(error: "File size is too large");

                profileImageFileName = $"{Path.GetFileNameWithoutExtension(employeeDetail.ProfileImage.FileName)}_{Guid.NewGuid()}{extension}";
            }

            var updateUserResponse = await _userService.UpdateUserAsync(employee.UserId, new UpdateApplicationUser
            {
                FirstName = employeeDetail.FirstName?.Trim(),
                LastName = employeeDetail.LastName?.Trim(),
                ProfilePicture = Path.Combine(usersImagesFolderPath, profileImageFileName).Replace("\\", "/"),
            });

            if (!updateUserResponse.IsSuccess)
                return new EntityApiResponse<EmployeeDetail>(error: updateUserResponse.Error);

            employee.Address = employeeDetail.Address?.Trim();
            employee.StreetAddress = employeeDetail.StreetAddress?.Trim();
            employee.City = employeeDetail.City?.Trim();
            employee.Description = employeeDetail.Description?.Trim();
            employee.BirthDate = employeeDetail.Birthdate.ToUniversalTime();
            employee.ModifiedById = currentUserId;
            employee.LastModifiedDate = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);
            employee.User = updateUserResponse.Entity;

            if (uploadedFile)
            {
                using (Stream stream = new FileStream(updateUserResponse.Entity.ProfilePicture, FileMode.Create))
                    await employeeDetail.ProfileImage.CopyToAsync(stream);

                if (oldProfilePictureFileName != "default.png")
                {
                    var fileToDelete = Path.Combine(usersImagesFolderPath, oldProfilePictureFileName);
                    File.Delete(fileToDelete);
                }
            }

            return new EntityApiResponse<EmployeeDetail>(entity: new EmployeeDetail(employee));
        }

        #endregion
    }
}
