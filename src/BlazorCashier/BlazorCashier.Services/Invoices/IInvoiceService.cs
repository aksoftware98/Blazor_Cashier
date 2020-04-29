using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Invoices
{
    public interface IInvoiceService
    {
        Task<EntitiesApiResponse<InvoiceDetail>> GetInvoicesForOrganizationAsync(string organizationId);
        Task<EntityApiResponse<InvoiceDetail>> GetInvoiceDetailsAsync(string invoiceId);
        Task<EntityApiResponse<InvoiceDetail>> AddInvoiceAsync(InvoiceDetail invoiceDetail, string currentUserId);
        Task<EntityApiResponse<InvoiceDetail>> UpdateInvoiceAsync(InvoiceDetail invoiceDetail, string currentUserId);
        Task<ApiResponse> DeleteInvoiceAsync(string invoiceId, string currentUserId);
    }
}
