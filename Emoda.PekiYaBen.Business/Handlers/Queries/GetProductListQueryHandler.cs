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
    public class GetProductListQueryHandler : IQueryHandler<GetProductListQuery, QueryListResult<ProductListItem>>
    {
        private string _SEARCH_QUERY = @"WITH TempResult AS(SELECT * FROM Product WHERE 1 = 1), TempCount AS (SELECT COUNT(*) as RecordCount FROM Product WHERE 1=1) SELECT * FROM TempResult, TempCount ORDER BY Type, SortOrder";


        public QueryListResult<ProductListItem> Retrieve(GetProductListQuery query)
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

                string queryStr = _SEARCH_QUERY;

                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var info = con.Query<ProductListItem>(queryStr, query, trans);

                    return new QueryListResult<ProductListItem>(info);
                }
            }
        }
    }
}
