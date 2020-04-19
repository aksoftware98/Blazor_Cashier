
namespace BlazorCashier.Services
{
    public interface IWebHostEnvironmentProvider
    {
        string WebRootPath { get; }
        string ContentRootPath { get; }
    }
}
