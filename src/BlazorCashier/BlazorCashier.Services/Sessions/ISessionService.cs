using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Sessions
{
    public interface ISessionService
    {
        Task<EntitiesApiResponse<SessionDetail>> GetSessionsForOrganizationAsync(string organizationId);
        Task<EntityApiResponse<WorkScheduleDetail>> GetWorkScheduleAsync(string organizationId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<EntityApiResponse<WorkScheduleDetail>> CreateWorkScheduleAsync(WorkScheduleDetail scheduleDetail, string currentUserId);
        Task<EntityApiResponse<WorkScheduleDetail>> UpdateWorkScheduleAsync(WorkScheduleDetail scheduleDetail, string currentUserId);
        Task<ApiResponse> DeleteSessionAsync(string sessionId);
    }
}
