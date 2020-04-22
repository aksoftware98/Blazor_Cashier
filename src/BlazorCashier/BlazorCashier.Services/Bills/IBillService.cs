using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Bills
{
    public interface IBillService
    {
        Task<EntitiesApiResponse<BillDetail>> GetBillsForOrganizationAsync(string organizationId);
        Task<EntityApiResponse<BillDetail>> GetBillDetailsAsync(string billId);
        Task<EntityApiResponse<BillDetail>> CreateBillAsync(BillDetail billDetail, string currentUseId);
        Task<EntityApiResponse<BillDetail>> UpdateBillAsync(BillDetail billDetail, string currentUseId);
        Task<ApiResponse> DeleteBillAsync(string billId);
    }
}
