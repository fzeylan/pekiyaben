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
    public class GetCoachCalendarListQueryHandler : IQueryHandler<GetCoachCalendarListQuery, QueryListResult<CoachCalendarInfo>>
    {
        private const string _SEARCH_QUERY = @"SELECT cc.*, u.FullName FROM CoachCalendar cc LEFT JOIN CoachCalendarUser ccu ON ccu.CoachCalendarId= cc.Id LEFT JOIN AppUser u ON u.Id = ccu.AppUserId LEFT JOIN [Transaction] t ON t.Id = ccu.TransactionId WHERE CoachId = @Id AND AvailableDate >= @StartDate AND AvailableDate <= @EndDate";

        public QueryListResult<CoachCalendarInfo> Retrieve(GetCoachCalendarListQuery query)
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
