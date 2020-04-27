using BlazorCashier.Models;
using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Sessions;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class SessionsController : BaseController
    {
        #region Private Members
        
        private readonly ISessionService _sessionService;

        #endregion

        #region Constructors

        public SessionsController(
            UserManager<ApplicationUser> userManager,
            ISessionService sessionService) : base(userManager)
        {
            _sessionService = sessionService;
        }

        #endregion

        #region Endpoints

        // GET: api/sessions
        /// <summary>
        /// Retrieves all the sessions for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntitiesApiResponse<Session>))]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUser();

            var sessionsResponse = await _sessionService.GetSessionsForOrganizationAsync(user.OrganizationId);

            return Ok(sessionsResponse);
        }

        // GET: api/sessions/workschedule
        /// <summary>
        /// Retrieves the work schedule for the current user according to a specific range of time
        /// </summary>
        /// <param name="request">Request which contains the range of time to retrieve the schedule for</param>
        /// <returns></returns>
        [HttpGet]
        [Route("workschedule")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<WorkScheduleDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<WorkScheduleDetail>))]
        public async Task<IActionResult> Get(WorkScheduleRequest request)
        {
            var user = await GetCurrentUser();

            var scheduleResponse = await
                _sessionService.GetWorkScheduleAsync(user.OrganizationId, request.FromDate, request.ToDate);

            if (!scheduleResponse.IsSuccess)
                return BadRequest(scheduleResponse);

            return Ok(scheduleResponse);
        }

        // POST: api/sessions/workschedule
        /// <summary>
        /// Creates a new work schedule with the passed sessions
        /// </summary>
        /// <param name="scheduleDetail">Schedule containing sessions to be added</param>
        /// <returns></returns>
        [HttpPost]
        [Route("workschedule")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<WorkScheduleDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<WorkScheduleDetail>))]
        public async Task<IActionResult> Post(WorkScheduleDetail scheduleDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();
            scheduleDetail.OrganiztionId = user.OrganizationId;

            var response = await _sessionService.CreateWorkScheduleAsync(scheduleDetail, user.Id);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }


        // PUT: api/sessions/workschedule
        /// <summary>
        /// Updates schedule with the passed new schedule
        /// </summary>
        /// <param name="scheduleDetail">New schedule containing new sessions to be added or updated</param>
        /// <returns></returns>
        [HttpPut]
        [Route("workschedule")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<WorkScheduleDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<WorkScheduleDetail>))]
        public async Task<IActionResult> Put(WorkScheduleDetail scheduleDetail)
        {
            var user = await GetCurrentUser();

            scheduleDetail.OrganiztionId = user.OrganizationId;

            var response = await _sessionService.UpdateWorkScheduleAsync(scheduleDetail, user.Id);

            if (!response.IsSuccess)
                return BadRequest(scheduleDetail);

            return Ok(response);
        }

        // DELETE: api/sessions/f7d8asy78
        /// <summary>
        /// Deletes a session by id
        /// </summary>
        /// <param name="sessionId">Id of the session to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{sessionId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string sessionId)
        {
            var response = await _sessionService.DeleteSessionAsync(sessionId);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        #endregion
    }
}
