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
    public class GetCategoryListQueryHandler : IQueryHandler<GetCategoryListQuery, QueryListResult<CategoryListItem>>
    {
        private string _SEARCH_QUERY = @"
            WITH TempResult AS(SELECT  * FROM ContentCategory), 
                 TempCount AS (SELECT COUNT(*) as RecordCount FROM ContentCategory)
            SELECT *
            FROM TempResult, TempCount
                %o%
            OFFSET @PageSize * (@PageNumber - 1) ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        public QueryListResult<CategoryListItem> Retrieve(GetCategoryListQuery query)
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

                string queryStr = _SEARCH_QUERY.Replace("%o%", query.OrderBy);

                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var info = con.Query<CategoryListItem>(queryStr, query, trans);

                    return new QueryListResult<CategoryListItem>(info);
                }
            }
        }
    }
}
