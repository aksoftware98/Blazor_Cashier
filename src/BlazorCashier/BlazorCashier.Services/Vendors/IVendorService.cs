using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Vendors
{
    public interface IVendorService
    {
        Task<EntitiesApiResponse<VendorDetail>> GetVendorsForOrganization(string organizationId);
        Task<EntityApiResponse<VendorDetail>> GetVendorDetails(string vendorId);
        //Task<EntitiesPagingApiResponse<VendorDetail>> SearchForVendorsByTextAsync(string searchText, string organizationId, int pageNumber = 0, int pageSize = 10);
        Task<EntityApiResponse<VendorDetail>> CreateVendorAsync(VendorDetail vendorDetail, string currentUserId);
        Task<EntityApiResponse<VendorDetail>> UpdateVendorAsync(VendorDetail vendorDetail, string currentUserId);
        Task<ApiResponse> DeleteVendor(string vendorId);
    }
}
