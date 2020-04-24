using BlazorCashier.Models.Identity;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Identity;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Account
{
    public interface IUserService
    {
        Task<IdentityApiResponse> LoginAsync(LoginRequest request);
        Task<ApiResponse> DeleteUserAsync(ApplicationUser user);
        Task<EntityApiResponse<ApplicationUser>> CreateUserAsync(CreateApplicationUser userDetail);
        Task<EntityApiResponse<ApplicationUser>> UpdateUserAsync(string userId, UpdateApplicationUser userDetail);
        Task<ApiResponse> ChangePasswordForUserAsync(ChangePasswordRequest request);
    }
}
