using OLCA.Infrastructure.CQS;
using System;
using System.Runtime.CompilerServices;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetCoachCalendarListQuery : IQuery
    {
        public GetCoachCalendarListQuery(int id, DateTime startDate, DateTime endDate)
        {
            this.Id = id;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
