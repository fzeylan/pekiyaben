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
    public class GetContentListQueryHandler : IQueryHandler<GetContentListQuery, QueryListResult<ContentListItem>>
    {
        //private string _SEARCH_QUERY = @"
        //    WITH TempResult AS(SELECT  c.*, cc.Title as Category FROM Content c INNER JOIN ContentCategory cc ON cc.Id = c.CategoryId WHERE 1 = 1 %s%), 
        //         TempCount AS (SELECT COUNT(*) as RecordCount  FROM Content c INNER JOIN ContentCategory cc ON cc.Id = c.CategoryId WHERE 1=1 %s% )
        //    SELECT *
        //    FROM TempResult, TempCount
        //        %o%
        //    OFFSET @PageSize * (@PageNumber - 1) ROWS
        //    FETCH NEXT @PageSize ROWS ONLY";


        private string _SEARCH_QUERY = @"
            SELECT c.Id, c.Title, Summary, cc.Title as Category, c.Status, CreateDate
            FROM Content c
			INNER JOIN ContentCategory cc ON cc.Id = c.CategoryId
            %s%
            %o%
            OFFSET @PageSize * (@PageNumber - 1) ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        public QueryListResult<ContentListItem> Retrieve(GetContentListQuery query)
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
                    sb.Append(@"WHERE c.Type=@Search");
                }

                string queryStr = _SEARCH_QUERY.Replace("%s%", sb.ToString()).Replace("%o%", query.OrderBy);

                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var info = con.Query<ContentListItem>(queryStr, query, trans);

                    return new QueryListResult<ContentListItem>(info);
                }
            }
        }
    }
}
