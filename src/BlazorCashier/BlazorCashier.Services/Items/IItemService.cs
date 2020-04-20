using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Items
{
    public interface IItemService
    {
        Task<EntitiesApiResponse<ItemDetail>> GetItemsForOrganization(string organizationId);
        Task<EntityApiResponse<ItemDetail>> GetItemDetails(string itemId);
        //Task<EntitiesPagingApiResponse<ItemDetail>> SearchForItemsByTextAsync(string searchText, string organizationId, int pageNumber = 0, int pageSize = 10);
        Task<EntityApiResponse<ItemDetail>> CreateItemAsync(ItemDetail itemDetail, string currentUserId);
        Task<EntityApiResponse<ItemDetail>> UpdateItemAsync(ItemDetail itemDetail, string currentUserId);
        Task<ApiResponse> DeleteItem(string itemId);
    }
}
