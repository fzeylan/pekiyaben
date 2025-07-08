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
    public class GetProductInfoListQueryHandler : IQueryHandler<GetProductInfoListQuery, QueryListResult<ProductInfo>>
    {
        private string _SEARCH_QUERY = @"WITH TempResult AS(SELECT 
        [Id]
      ,[Code]
      ,[Type]
      ,[Title]
      ,[Description]
      ,[File]
      ,[Price]
      ,[SortOrder]
      ,[ShowMainPage]
      ,[Status]
FROM Product WHERE 1 = 1), TempCount AS (SELECT COUNT(*) as RecordCount FROM Product WHERE 1=1) SELECT * FROM TempResult, TempCount";


        public QueryListResult<ProductInfo> Retrieve(GetProductInfoListQuery query)
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
                string queryStr = _SEARCH_QUERY.Replace("%s%", sb.ToString()).Replace("%o%", query.OrderBy);

                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var info = con.Query<ProductInfo>(queryStr, query, trans);

                    return new QueryListResult<ProductInfo>(info);
                }
            }
        }
    }
}
