using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Stocks
{
    public class StockService : IStockService
    {
        #region Private Members

        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Organization> _orgRepository;

        #endregion

        #region Constructors

        public StockService(
            IRepository<Stock> stockRepository,
            IRepository<Item> itemRepository,
            IRepository<Organization> orgRepository)
        {
            _stockRepository = stockRepository;
            _itemRepository = itemRepository;
            _orgRepository = orgRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<StockDetail>> CreateStockAsync(StockDetail stockDetail, string currentUserId)
        {
            if (stockDetail is null)
                throw new ArgumentNullException(nameof(stockDetail));

            // Check the item
            var item = await _itemRepository.GetByIdAsync(stockDetail.Item.Id);

            if (item is null)
                return new EntityApiResponse<StockDetail>(error: "Item does not exist");

            // Check if the item already has a stock attached to it
            var stockForItem = await _stockRepository.Table.FirstOrDefaultAsync(s => s.ItemId == item.Id);

            if (!(stockForItem is null))
                return new EntityApiResponse<StockDetail>(error: "Item already has a stock");

            var stock = new Stock
            {
                Quantity = stockDetail.Quantity,
                Price = stockDetail.Price,
                SellingPrice = stockDetail.SellingPrice,
                Points = stockDetail.Points,
                OrganizationId = stockDetail.OrganiationId,
                CreatedById = currentUserId,
                ModifiedById = currentUserId,
                ItemId = item.Id
            };

            await _stockRepository.InsertAsync(stock);

            return new EntityApiResponse<StockDetail>(entity: new StockDetail(stock));
        }

        public async Task<ApiResponse> DeleteStockAsync(string stockId)
        {
            var stock = await _stockRepository.GetByIdAsync(stockId);

            if (stock is null)
                return new ApiResponse("Stock does not exist");

            await _stockRepository.DeleteAsync(stock);

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<StockDetail>> GetStockDetailsAsync(string stockId)
        {
            var stock = await _stockRepository.GetByIdAsync(stockId);

            if (stock is null)
                return new EntityApiResponse<StockDetail>(error: "Stock does not exist");

            return new EntityApiResponse<StockDetail>(entity: new StockDetail(stock));
        }

        public async Task<EntitiesApiResponse<StockDetail>> GetStocksForOrganizationAsync(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<StockDetail>(error: "Organization does not exist");

            var stocks = await _stockRepository.Table
                .Where(s => s.OrganizationId == org.Id).ToListAsync();

            var stocksDetails = stocks.Select(s => new StockDetail(s));

            return new EntitiesApiResponse<StockDetail>(entities: stocksDetails);
        }

        public async Task<EntityApiResponse<StockDetail>> UpdateStockAsync(StockDetail stockDetail, string currentUserId)
        {
            if (stockDetail is null)
                throw new ArgumentNullException(nameof(stockDetail));

            var stock = await _stockRepository.GetByIdAsync(stockDetail.Id);

            if (stock is null)
                return new EntityApiResponse<StockDetail>(error: "Stock does not exist");

            // Check the item
            var item = await _itemRepository.GetByIdAsync(stockDetail.Item.Id);

            if (item is null)
                return new EntityApiResponse<StockDetail>(error: "Item does not exist");

            // Check if the item already has a stock attached to it other than the current one
            var stockForItem = await _stockRepository.Table.FirstOrDefaultAsync(s => s.ItemId == item.Id && s.Id != stock.Id);

            if (!(stockForItem is null))
                return new EntityApiResponse<StockDetail>(error: "Item already has a stock");

            stock.Quantity = stockDetail.Quantity;
            stock.Price = stockDetail.Price;
            stock.SellingPrice = stockDetail.SellingPrice;
            stock.Points = stockDetail.Points;
            stock.LastModifiedDate = DateTime.UtcNow;
            stock.ModifiedById = currentUserId;

            await _stockRepository.UpdateAsync(stock);

            return new EntityApiResponse<StockDetail>(entity: new StockDetail(stock));
        }

        #endregion
    }
}
