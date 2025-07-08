using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using Dapper;
using System.Text;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetCoachListQueryHandler : IQueryHandler<GetCoachListQuery, QueryListResult<CoachListItem>>
    {
        private string _SEARCH_QUERY = @"
            WITH TempResult AS(SELECT * FROM Coach WHERE 1 = 1 %s%), TempCount AS (SELECT COUNT(*) as RecordCount FROM Coach WHERE 1=1 %s% )
          SELECT *
            FROM TempResult, TempCount
             %o%
          OFFSET @PageSize * (@PageNumber - 1) ROWS
      FETCH NEXT @PageSize ROWS ONLY";


        public QueryListResult<CoachListItem> Retrieve(GetCoachListQuery query)
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
                    sb.Append(@"AND (FullName LIKE '%' + @Search + '%' OR Email LIKE '%' + @Search + '%')");
                }

                string queryStr = _SEARCH_QUERY.Replace("%s%", sb.ToString()).Replace("%o%", query.OrderBy);

                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.Query<CoachListItem>(queryStr, query, trans);

                    return new QueryListResult<CoachListItem>(userInfo);
                }
            }
        }
    }
}
