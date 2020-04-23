using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Discounts
{
    public class DiscountService : IDiscountService
    {
        #region Private Members

        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<DiscountItem> _discountItemRepository;
        private readonly IRepository<Organization> _orgRepository;
        private readonly IRepository<Stock> _stockRepository;

        #endregion

        #region Constructors

        public DiscountService(
            IRepository<Discount> discountRepository,
            IRepository<DiscountItem> discountItemRepository,
            IRepository<Organization> orgRepository,
            IRepository<Stock> stockRepository)
        {
            _discountRepository = discountRepository;
            _discountItemRepository = discountItemRepository;
            _orgRepository = orgRepository;
            _stockRepository = stockRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<DiscountDetail>> CreateDiscountAsync(DiscountDetail discountDetail, string currentUserId)
        {
            if (discountDetail is null)
                throw new ArgumentNullException(nameof(discountDetail));

            if (discountDetail.DiscountItems is null || discountDetail.DiscountItems.Count < 1)
                return new EntityApiResponse<DiscountDetail>(error: "Discount has no items");

            if (discountDetail.StartDate >= discountDetail.EndDate)
                return new EntityApiResponse<DiscountDetail>(error: "The start date should be earlier than end date");

            var org = await _orgRepository.GetByIdAsync(discountDetail.OrganizationId);

            if (org is null)
                return new EntityApiResponse<DiscountDetail>(error: "Organization does not exist");

            var discount = new Discount
            {
                Description = discountDetail.Description?.Trim(),
                Value = discountDetail.Value,
                StartDate = discountDetail.StartDate.ToUniversalTime(),
                EndDate = discountDetail.EndDate.ToUniversalTime(),
                CreatedById = currentUserId,
                ModifiedById = currentUserId,
                OrganizationId = org.Id
            };

            await _discountRepository.InsertAsync(discount);

            foreach (var discountItem in discountDetail.DiscountItems)
            {
                if (discountItem.Stock is null)
                    continue;

                var stock = _stockRepository.TableNoTracking.FirstOrDefault(s => s.Id == discountItem.Stock.Id);

                if (stock is null)
                    continue;

                var newDiscountItem = new DiscountItem
                {
                    StockId = stock.Id,
                    DiscountId = discount.Id,
                    CreatedById = currentUserId,
                    ModifiedById = currentUserId,
                    OrganizationId = org.Id
                };

                await _discountItemRepository.InsertAsync(newDiscountItem);
            }

            return new EntityApiResponse<DiscountDetail>(entity: new DiscountDetail(discount));
        }

        public async Task<ApiResponse> DeleteDiscountAsync(string discountId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);

            if (discount is null)
                return new ApiResponse("Discount does not exist");

            await _discountRepository.DeleteAsync(discount);

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<DiscountDetail>> GetDiscountDetailsAsync(string discountId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);

            if (discount is null)
                return new EntityApiResponse<DiscountDetail>(error: "Discount does not exist");

            return new EntityApiResponse<DiscountDetail>(entity: new DiscountDetail(discount));
        }

        public async Task<EntitiesApiResponse<DiscountDetail>> GetDiscountsForOrganizationAsync(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<DiscountDetail>(error: "Organization does not exist");

            var discounts = await _discountRepository.Table
                .Where(d => d.OrganizationId == org.Id).ToListAsync();

            var discountDetails = discounts.Select(d => new DiscountDetail(d));

            return new EntitiesApiResponse<DiscountDetail>(entities: discountDetails);
        }

        public async Task<EntityApiResponse<DiscountDetail>> UpdateDiscountAsync(DiscountDetail discountDetail, string currentUserId)
        {
            if (discountDetail is null)
                throw new ArgumentNullException(nameof(discountDetail));

            if (discountDetail.DiscountItems is null || discountDetail.DiscountItems.Count < 1)
                return new EntityApiResponse<DiscountDetail>(error: "Discount has no items");

            if (discountDetail.StartDate >= discountDetail.EndDate)
                return new EntityApiResponse<DiscountDetail>(error: "The start date should be earlier than the end date");

            var discount = await _discountRepository.GetByIdAsync(discountDetail.Id);

            if (discount is null)
                return new EntityApiResponse<DiscountDetail>(error: "Discount does not exist");

            var org = await _orgRepository.GetByIdAsync(discountDetail.OrganizationId);

            if (org is null)
                return new EntityApiResponse<DiscountDetail>(error: "Organization does not exist");

            // Delete the current items for the discount
            await _discountItemRepository.DeleteAsync(discount.DiscountItems);

            // Add the new ones to the discount
            foreach (var discountItem in discountDetail.DiscountItems)
            {
                var stock = await _stockRepository.GetByIdAsync(discountItem.Stock?.Id);

                if (stock is null)
                    continue;

                var newDiscountItem = new DiscountItem
                {
                    StockId = stock.Id,
                    DiscountId = discount.Id,
                    CreatedById = currentUserId,
                    ModifiedById = currentUserId,
                    OrganizationId = org.Id
                };

                await _discountItemRepository.InsertAsync(newDiscountItem);
            }

            discount.StartDate = discountDetail.StartDate.ToUniversalTime();
            discount.EndDate = discountDetail.EndDate.ToUniversalTime();
            discount.ModifiedById = currentUserId;
            discount.LastModifiedDate = DateTime.UtcNow;
            discount.Value = discountDetail.Value;
            discount.Description = discountDetail.Description?.Trim();

            await _discountRepository.UpdateAsync(discount);

            return new EntityApiResponse<DiscountDetail>(entity: new DiscountDetail(discount));
        }

        #endregion
    }
}
