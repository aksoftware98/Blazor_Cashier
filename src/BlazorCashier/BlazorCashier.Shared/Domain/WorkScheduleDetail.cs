using BlazorCashier.Models;
using BlazorCashier.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorCashier.Shared.Domain
{
    public class WorkScheduleDetail
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string OrganiztionId { get; set; }
        public ICollection<SessionDetail> Sessions { get; set; }

        public WorkScheduleDetail()
        {

        }

        public WorkScheduleDetail(ICollection<Session> sessions, DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Sessions = sessions.Select(s => new SessionDetail(s)).ToList();
        }
    }

    public class SessionDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalTotal { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal ProfitTotal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ApplicationUserDetail User { get; set; }
        public string OrganizationId { get; set; }

        public SessionDetail()
        {

        }

        public SessionDetail(Session session)
        {
            Id = session.Id;
            Name = session.Name;
            Description = session.Description;
            OriginalTotal = session.OriginalTotal;
            FinalTotal = session.FinalTotal;
            ProfitTotal = session.ProfitTotal;
            StartDate = session.StartDate;
            EndDate = session.EndDate;
            User = new ApplicationUserDetail(session.User);
        }

    }

    public class ApplicationUserDetail
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ApplicationUserDetail()
        {

        }

        public ApplicationUserDetail(ApplicationUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}
