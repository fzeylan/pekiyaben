using Dapper;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Coaching;
using Emoda.PekiYaBen.Entity.Content;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetContentCategoryInfoQueryHandler : IQueryHandler<GetContentCategoryInfoQuery, QuerySingleResult<ContentCategoryInfo>>
    {
        public QuerySingleResult<ContentCategoryInfo> Retrieve(GetContentCategoryInfoQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var info = con.Query<ContentCategoryInfo>("SELECT * FROM ContentCategory WHERE Id = @Id", query, trans).SingleOrDefault();

                    return new QuerySingleResult<ContentCategoryInfo>(info);
                }
            }
        }
    }
}
