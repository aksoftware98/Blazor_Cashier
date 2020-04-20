using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Items
{
    public class ItemService : IItemService
    {
        #region Private Members

        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Organization> _orgRepository;

        #endregion

        #region Constructors

        public ItemService(
            IRepository<Item> itemRepository,
            IRepository<Organization> orgRepository)
        {
            _itemRepository = itemRepository;
            _orgRepository = orgRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<ItemDetail>> CreateItemAsync(ItemDetail itemDetail, string currentUserId)
        {
            if (itemDetail is null)
                throw new ArgumentNullException(nameof(itemDetail));

            var newItem = new Item
            {
                Name = itemDetail.Name.Trim(),
                Description = itemDetail.Description.Trim(),
                Barcode = itemDetail.Barcode.Trim(),
                CountryOfOrigin = itemDetail.CountryOfOrigin.Trim(),
                AdditionalProperties = itemDetail.AdditionalProperties.Trim(),
                Points = itemDetail.Points,
                Price = itemDetail.Price,
                SellingPrice = itemDetail.SellingPrice,
                OrganizationId = itemDetail.OrganizationId,
                CreatedById = currentUserId,
                ModifiedById = currentUserId
            };

            await _itemRepository.InsertAsync(newItem);

            return new EntityApiResponse<ItemDetail>(entity: new ItemDetail(newItem));
        }

        public async Task<ApiResponse> DeleteItem(string itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);

            if (item == null)
                return new ApiResponse("Item does not exist");

            await _itemRepository.DeleteAsync(item);

            return new ApiResponse();
        }

        //public async Task<EntitiesPagingApiResponse<ItemDetail>> SearchForItemsByTextAsync(string searchText, string organizationId, int pageNumber = 0, int pageSize = 10)
        //{
        //    var org = await _orgRepository.GetByIdAsync(organizationId);

        //    if (org is null)
        //        return new EntitiesPagingApiResponse<ItemDetail>(error: "Organiztion does not exist");

        //    var query = _itemRepository.TableNoTracking;

        //    query = query.Where(item => item.OrganizationId == org.Id);

        //    if (!string.IsNullOrEmpty(searchText))
        //        query = query
        //            .Where(item => item.Name.Contains(searchText) ||
        //                           item.Description.Contains(searchText) ||
        //                           item.CountryOfOrigin.Contains(searchText));

        //    query = query.Skip(pageNumber * pageSize).Take(pageSize);

        //    var items = await query.ToListAsync();

        //    var itemsDetails = items.Select(i => new ItemDetail(i));

        //    return new EntitiesPagingApiResponse<ItemDetail>
        //        (entities: itemsDetails,
        //         totalResults: items.Count,
        //         pageNumber: pageNumber,
        //         pageSize: pageSize);
        //}

        public async Task<EntityApiResponse<ItemDetail>> GetItemDetails(string itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);

            if (item is null)
                return new EntityApiResponse<ItemDetail>(error: "Item does not exist");

            return new EntityApiResponse<ItemDetail>(entity: new ItemDetail(item));
        }

        public async Task<EntitiesApiResponse<ItemDetail>> GetItemsForOrganization(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<ItemDetail>(error: "Organization does not exist");

            var items = await _itemRepository.Table
                .Where(i => i.OrganizationId == organizationId).ToListAsync();

            var itemDetails = items.Select(i => new ItemDetail(i));

            return new EntitiesApiResponse<ItemDetail>(entities: itemDetails);
        }

        public async Task<EntityApiResponse<ItemDetail>> UpdateItemAsync(ItemDetail itemDetail, string currentUserId)
        {
            if (itemDetail is null)
                throw new ArgumentNullException(nameof(itemDetail));

            var item = await _itemRepository.GetByIdAsync(itemDetail.Id);

            if (item is null)
                return new EntityApiResponse<ItemDetail>(error: "Item does not exist");

            item.Name = itemDetail.Name.Trim();
            item.Description = itemDetail.Description.Trim();
            item.CountryOfOrigin = itemDetail.CountryOfOrigin.Trim();
            item.Barcode = itemDetail.Barcode.Trim();
            item.Points = itemDetail.Points;
            item.Price = itemDetail.Price;
            item.SellingPrice = itemDetail.SellingPrice;
            item.AdditionalProperties = itemDetail.AdditionalProperties.Trim();
            item.ModifiedById = currentUserId;
            item.LastModifiedDate = DateTime.UtcNow;

            await _itemRepository.UpdateAsync(item);

            return new EntityApiResponse<ItemDetail>(entity: new ItemDetail(item));
        }
        
        #endregion
    }
}
