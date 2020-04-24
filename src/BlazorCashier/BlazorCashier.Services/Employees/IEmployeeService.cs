using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Employees
{
    public interface IEmployeeService
    {
        Task<EntitiesApiResponse<EmployeeDetail>> GetEmployeesForOrganizationAsync(string organizationId);
        Task<EntityApiResponse<EmployeeDetail>> GetEmployeeDetailsAsync(string employeeId);
        Task<EntityApiResponse<EmployeeDetail>> CreateEmployeeAsync(EmployeeDetail employeeDetail, string currentUserId);
        Task<EntityApiResponse<EmployeeDetail>> UpdateEmployeeAsync(EmployeeDetail employeeDetail, string currentUserId);
        Task<ApiResponse> DeleteEmployeeAsync(string employeeId);
    }
}
