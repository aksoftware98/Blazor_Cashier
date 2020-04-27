using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Models.Extensions;
using BlazorCashier.Models.Identity;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Sessions
{
    public class SessionService : ISessionService
    {
        #region Private Members

        private readonly IRepository<Session> _sessionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Organization> _orgRepository;

        #endregion

        #region Constructors

        public SessionService(
            IRepository<Session> sessionRepository,
            UserManager<ApplicationUser> userManager,
            IRepository<Organization> orgRepository)
        {
            _sessionRepository = sessionRepository;
            _userManager = userManager;
            _orgRepository = orgRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<WorkScheduleDetail>> CreateWorkScheduleAsync(WorkScheduleDetail scheduleDetail, string currentUserId)
        {
            if (scheduleDetail is null)
                throw new ArgumentNullException(nameof(scheduleDetail));

            if (scheduleDetail.Sessions is null || scheduleDetail.Sessions.Count < 1)
                return new EntityApiResponse<WorkScheduleDetail>(error: "Sessions can't be empty");

            // if there exists a session with startDate DOW different that endDate DOW
            if (scheduleDetail.Sessions.Any(s => s.StartDate.DayOfWeek != s.EndDate.DayOfWeek))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Sessions start and end date should be on the same day");

            // Check if there is a session with end date earlier than start date
            if (scheduleDetail.Sessions.Any(s => s.EndDate <= s.StartDate))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Schedule has incorrect time ranges");

            var tomorrowDate = DateTime.UtcNow.AddDays(1).NormalizedDate();

            if (scheduleDetail.Sessions.Any(s => s.StartDate < tomorrowDate))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Sessions can only be added for tomorrow and beyond");

            if (scheduleDetail.Sessions.Any(s => s.User is null))
                return new EntityApiResponse<WorkScheduleDetail>(error: "A session doesn't have a user specified for it");

            var sessionsByDayOfWeek = scheduleDetail.Sessions.GroupBy(s => s.StartDate.DayOfWeek);
            
            // Check if any session has conflicts wit any other session
            if (HasTimeConflicts(sessionsByDayOfWeek))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Schedule has some time conflicts");

            var org = await _orgRepository.GetByIdAsync(scheduleDetail.OrganiztionId);

            if (org is null)
                return new EntityApiResponse<WorkScheduleDetail>(error: "Organization does not exist");

            var usersIdsRetrieved = new List<string>();
            var sessionsAdded = new Session[scheduleDetail.Sessions.Count];
            int i = 0;

            foreach (var sessionDetail in scheduleDetail.Sessions)
            {
                if (!usersIdsRetrieved.Contains(sessionDetail.User.Id))
                {
                    var user = await _userManager.FindByIdAsync(sessionDetail.User.Id);

                    if (user is null)
                        return new EntityApiResponse<WorkScheduleDetail>
                            (error: $"Session with name: {sessionDetail.Name} has a selected user that does not exist");

                    usersIdsRetrieved.Add(user.Id);
                }

                var session = new Session
                {
                    Name = sessionDetail.Name?.Trim(),
                    Description = sessionDetail.Description?.Trim(),
                    StartDate = sessionDetail.StartDate.ToUniversalTime(),
                    EndDate = sessionDetail.EndDate.ToUniversalTime(),
                    CreatedById = currentUserId,
                    ModifiedById = currentUserId,
                    OrganizationId = org.Id,
                    UserId = sessionDetail.User.Id
                };

                await _sessionRepository.InsertAsync(session);
                sessionsAdded[i] = session;
                i++;
            }

            var endDate = sessionsAdded.Max(s => s.EndDate);

            return new EntityApiResponse<WorkScheduleDetail>(entity: new WorkScheduleDetail(sessionsAdded, tomorrowDate, endDate));
        }

        public async Task<EntityApiResponse<WorkScheduleDetail>> UpdateWorkScheduleAsync(WorkScheduleDetail scheduleDetail, string currentUserId)
        {
            if (scheduleDetail is null)
                throw new ArgumentNullException(nameof(scheduleDetail));

            if (scheduleDetail.Sessions is null || scheduleDetail.Sessions.Count < 1)
                return new EntityApiResponse<WorkScheduleDetail>(error: "Sessions can't be empty");

            // if there exists a session with startDate DOW different that endDate DOW
            if (scheduleDetail.Sessions.Any(s => s.StartDate.DayOfWeek != s.EndDate.DayOfWeek))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Sessions start and end date should be on the same day");

            // Check if there is a session with end date earlier than start date
            if (scheduleDetail.Sessions.Any(s => s.EndDate <= s.StartDate))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Schedule has incorrect time ranges");

            var tomorrorwDate = DateTime.UtcNow.AddDays(1).NormalizedDate();

            if (scheduleDetail.Sessions.Any(s => s.StartDate < tomorrorwDate))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Can't update sessions earlier than tomorrow");

            if (scheduleDetail.Sessions.Any(s => s.User is null))
                return new EntityApiResponse<WorkScheduleDetail>(error: "A session doesn't have a user specified for it");

            var newSessionsByDayOfWeek = scheduleDetail.Sessions.GroupBy(s => s.StartDate.DayOfWeek);

            // Check if any session has conflicts wit any other session
            if (HasTimeConflicts(newSessionsByDayOfWeek))
                return new EntityApiResponse<WorkScheduleDetail>(error: "Schedule has some time conflicts");

            var endDate = scheduleDetail.Sessions.Max(s => s.EndDate);

            var oldSessions = await SessionsBetweenDates(tomorrorwDate, endDate, scheduleDetail.OrganiztionId);

            // Get from the old session the ones where they do not exist in the new sessions
            // Those would be the ones to delete
            var sessionsToDelete = oldSessions.Where(session => !scheduleDetail.Sessions.Any(sessionDetail => sessionDetail.Id == session.Id));

            var usersIdsRetrieved = new List<string>();
            var sessionsToReturn = new Session[scheduleDetail.Sessions.Count];
            int i = 0;

            foreach (var sessionDetail in scheduleDetail.Sessions)
            {
                if (!usersIdsRetrieved.Contains(sessionDetail.User.Id))
                {
                    var user = await _userManager.FindByIdAsync(sessionDetail.User.Id);

                    if (user is null)
                        return new EntityApiResponse<WorkScheduleDetail>
                            (error: $"Session with name: {sessionDetail.Name} has a selected user that does not exist");

                    usersIdsRetrieved.Add(user.Id);
                }

                Session session;

                // If the id is null then it is a new session to add
                if (string.IsNullOrEmpty(sessionDetail.Id))
                {
                    session = new Session
                    {
                        Name = sessionDetail.Name?.Trim(),
                        Description = sessionDetail.Description?.Trim(),
                        StartDate = sessionDetail.StartDate.ToUniversalTime(),
                        EndDate = sessionDetail.EndDate.ToUniversalTime(),
                        CreatedById = currentUserId,
                        ModifiedById = currentUserId,
                        OrganizationId = scheduleDetail.OrganiztionId,
                        UserId = sessionDetail.User.Id
                    };
                    
                    await _sessionRepository.InsertAsync(session);
                }

                // It is an already added session that needs to be updated
                else
                {
                    session = oldSessions.FirstOrDefault(s => s.Id == sessionDetail.Id);

                    if (session is null)
                        continue;

                    session.StartDate = sessionDetail.StartDate.ToUniversalTime();
                    session.EndDate = sessionDetail.EndDate.ToUniversalTime();
                    session.Name = sessionDetail.Name?.Trim();
                    session.Description = sessionDetail.Description.Trim();
                    session.UserId = sessionDetail.User.Id;
                    session.LastModifiedDate = DateTime.UtcNow;
                    session.ModifiedById = currentUserId;

                    await _sessionRepository.UpdateAsync(session);
                }

                sessionsToReturn[i] = session;
                i++;
            }

            // Delete the sessions that have been excluded from the schedule
            await _sessionRepository.DeleteAsync(sessionsToDelete);

            return new EntityApiResponse<WorkScheduleDetail>(entity: new WorkScheduleDetail(sessionsToReturn, tomorrorwDate, endDate));
        }

        public async Task<ApiResponse> DeleteSessionAsync(string sessionId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);

            if (session is null)
                return new ApiResponse("Session does not exist");

            if (session.CashierPayments.Any())
                return new ApiResponse("Session can't be deleted. It has some payments related to it");

            await _sessionRepository.DeleteAsync(session);

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<WorkScheduleDetail>> GetWorkScheduleAsync(string organizationId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (fromDate.HasValue && toDate.HasValue && fromDate.Value > toDate.Value)
                return new EntityApiResponse<WorkScheduleDetail>(error: "Incorrect selection of dates");

            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntityApiResponse<WorkScheduleDetail>(error: "Organization does not exist");

            var startDate = fromDate ?? DateTime.UtcNow.AddDays(1).NormalizedDate();
            var endDate = toDate ?? startDate.AddDays(7);

            var sessions = await SessionsBetweenDates(startDate, endDate, org.Id);

            return new EntityApiResponse<WorkScheduleDetail>(entity: new WorkScheduleDetail(sessions, startDate, endDate));
        }

        public async Task<EntitiesApiResponse<SessionDetail>> GetSessionsForOrganizationAsync(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<SessionDetail>(error: "Organization does not exist");

            var sessions = await _sessionRepository.Table
                .Where(s => s.OrganizationId == org.Id).ToListAsync();

            var sessionsDetails = sessions.Select(s => new SessionDetail(s));

            return new EntitiesApiResponse<SessionDetail>(entities: sessionsDetails);
        }

        #endregion

        #region Helper Methods

        private bool HasTimeConflicts(IEnumerable<IGrouping<DayOfWeek, SessionDetail>> sessionsByDayOfWeek)
        {
            foreach (var dayOfWeekSessions in sessionsByDayOfWeek)
            {
                var sessionsStack = dayOfWeekSessions.OrderByDescending(s => s.StartDate).ToStack();

                while (sessionsStack.Count > 0)
                {
                    var session = sessionsStack.Pop();

                    // Basically if there is a session where its startDate is earlier than the popped session endDate
                    if (sessionsStack.Any(s => session.EndDate > s.StartDate))
                        return true;
                }
            }

            return false;
        }

        private async Task<ICollection<Session>> SessionsBetweenDates(DateTime fromDAte, DateTime toDate, string organizationId)
        {
            return await _sessionRepository.Table
                .Where(s => s.StartDate >= fromDAte && s.EndDate <= toDate && s.OrganizationId == organizationId).ToListAsync();
        }

        #endregion
    }
}
