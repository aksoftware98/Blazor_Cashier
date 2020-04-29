using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Discounts
{
    public interface IDiscountService
    {
        Task<EntitiesApiResponse<DiscountDetail>> GetDiscountsForOrganizationAsync(string organizationId);
        Task<EntityApiResponse<DiscountDetail>> GetDiscountDetailsAsync(string discountId);
        Task<EntityApiResponse<DiscountDetail>> GetMaxDiscountForStockAsync(string stockId);
        Task<EntityApiResponse<DiscountDetail>> CreateDiscountAsync(DiscountDetail discountDetail, string currentUserId);
        Task<EntityApiResponse<DiscountDetail>> UpdateDiscountAsync(DiscountDetail discountDetail, string currentUserId);
        Task<ApiResponse> DeleteDiscountAsync(string discountId);
    }
}
