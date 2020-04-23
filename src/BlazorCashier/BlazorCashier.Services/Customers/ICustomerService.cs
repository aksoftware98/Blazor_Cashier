using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Customers
{
    public interface ICustomerService
    {
        Task<EntitiesApiResponse<CustomerDetail>> GetCustomersForOrganizationAsync(string organizationId);
        Task<EntityApiResponse<CustomerDetail>> GetCustomerDetailsAsync(string customerId);
        Task<EntityApiResponse<CustomerDetail>> CreateCustomerAsync(CustomerDetail customerDetail, string currentUserId);
        Task<EntityApiResponse<CustomerDetail>> UpdateCustomerAsync(CustomerDetail customerDetail, string currentUserId);
        Task<EntityApiResponse<CustomerDetail>> AlterPointsOfCustomerAsync(string customerId, int points);
        Task<ApiResponse> DeleteCustomerAsync(string customerId);
    }
}
