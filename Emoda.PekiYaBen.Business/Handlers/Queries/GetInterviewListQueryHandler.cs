using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using Dapper;
using System.Text;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetInterviewListQueryHandler : IQueryHandler<GetInterviewListQuery, QueryListResult<InterviewListItem>>
    {
        private string _SEARCH_QUERY = @"
            WITH TempResult AS(
                SELECT c.FullName CoachName, cc.AvailableDate, ccu.InterviewType, ccu.Status, u.FullName UserName, t.TransactionDate, t.TransactionPrice, t.MarketId
                    FROM 
	                    CoachCalendar cc 
                    
                    LEFT JOIN  CoachCalendarUser ccu ON ccu.CoachCalendarId=cc.Id
                    LEFT JOIN AppUser u ON u.Id = ccu.AppUserId
                    LEFT JOIN [Transaction] t ON t.Id = ccu.TransactionId 
                    LEFT JOIN Coach c ON c.Id=cc.CoachId

                    Where ccu.TransactionId IS NOT null %s%), TempCount AS (SELECT COUNT(*) as RecordCount FROM 
	                    CoachCalendar cc 
	                    LEFT JOIN  CoachCalendarUser ccu ON ccu.CoachCalendarId=cc.Id
                        LEFT JOIN AppUser u ON u.Id = ccu.AppUserId
                        LEFT JOIN [Transaction] t ON t.Id = ccu.TransactionId 
                        LEFT JOIN Coach c ON c.Id=cc.CoachId
                    Where ccu.TransactionId IS NOT null%s% )
          SELECT *
            FROM TempResult, TempCount
             %o%
          OFFSET @PageSize * (@PageNumber - 1) ROWS
      FETCH NEXT @PageSize ROWS ONLY";


        public QueryListResult<InterviewListItem> Retrieve(GetInterviewListQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                if (query.PageSize == 0)
                {
                    query.PageSize = 10;
                }

                if (query.PageNumber == 0)
                {
                    query.PageNumber = 1;
                }

                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(query.Search))
                {
                    sb.Append(@"AND (CoachName LIKE '%' + @Search + '%')");
                }

                string queryStr = _SEARCH_QUERY.Replace("%s%", sb.ToString()).Replace("%o%", query.OrderBy);

                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var info = con.Query<InterviewListItem>(queryStr, query, trans);

                    return new QueryListResult<InterviewListItem>(info);
                }
            }
        }
    }
}
