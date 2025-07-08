using Dapper;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetFAQListQueryHandler : IQueryHandler<GetFAQListQuery, QueryListResult<FAQListItem>>
    {
        private string _SEARCH_QUERY = @"WITH TempResult AS(SELECT Id, SortOrder, Title, Description, Status, CreatedDate FROM FAQ  WHERE 1 = 1), TempCount AS (SELECT COUNT(*) as RecordCount FROM FAQ WHERE 1=1) SELECT * FROM TempResult, TempCount ORDER BY CreatedDate desc";


        public QueryListResult<FAQListItem> Retrieve(GetFAQListQuery query)
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
                    var info = con.Query<FAQListItem>(queryStr, query, trans);

                    return new QueryListResult<FAQListItem>(info);
                }
            }
        }
    }
}
