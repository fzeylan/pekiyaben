using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using Dapper;
using System.Text;
using Emoda.PekiYaBen.Entity.Coaching;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetCalendarListQueryHandler : IQueryHandler<GetCalendarListQuery, QueryListResult<CoachCalendarInfo>>
    {
        private const string _SEARCH_QUERY = @"SELECT AvailableDate, 0 AS TransactionId FROM CoachCalendar GROUP BY AvailableDate ORDER BY AvailableDate";

        public QueryListResult<CoachCalendarInfo> Retrieve(GetCalendarListQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();

                using (var trans = con.BeginTransaction())
                {
                    var result = con.Query<CoachCalendarInfo>(_SEARCH_QUERY, query, trans);

                    return new QueryListResult<CoachCalendarInfo>(result);
                }
            }
        }
    }
}
