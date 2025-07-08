using OLCA.Infrastructure.CQS;
using System;
using System.Runtime.CompilerServices;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetCalendarListQuery : IQuery
    {
        public GetCalendarListQuery(int id, DateTime startDate, DateTime endDate)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
