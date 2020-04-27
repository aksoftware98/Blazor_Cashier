using System;

namespace BlazorCashier.Shared.Domain
{
    public class WorkScheduleRequest
    {
        public DateTime? FromDate{ get; set; }
        public DateTime? ToDate { get; set; }

        public WorkScheduleRequest()
        {

        }

        public WorkScheduleRequest(DateTime? fromDate, DateTime? toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}
