using BlazorCashier.Shared;
using BlazorCashier.Shared.Identity;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Account
{
    public interface IUserService
    {
        Task<IdentityApiResponse> LoginAsync(LoginRequest request);
    }
}
