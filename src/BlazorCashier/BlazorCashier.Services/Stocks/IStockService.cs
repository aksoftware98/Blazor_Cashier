using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Stocks
{
    public interface IStockService
    {
        Task<EntitiesApiResponse<StockDetail>> GetStocksForOrganizationAsync(string organizationId);
        Task<EntityApiResponse<StockDetail>> GetStockDetailsAsync(string stockId);
        Task<EntityApiResponse<StockDetail>> CreateStockAsync(StockDetail StockDetail, string currentUserId);
        Task<EntityApiResponse<StockDetail>> UpdateStockAsync(StockDetail stockDetail, string currentUserId);
        Task<ApiResponse> DeleteStockAsync(string stockId);
    }
}
