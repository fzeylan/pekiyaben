using Dapper;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Coaching;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetProductInfoQueryHandler : IQueryHandler<GetProductInfoQuery, QuerySingleResult<ProductInfo>>
    {
        public QuerySingleResult<ProductInfo> Retrieve(GetProductInfoQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.Query<ProductInfo>("SELECT * FROM Product WHERE Id = @Id", query, trans).SingleOrDefault();

                    return new QuerySingleResult<ProductInfo>(userInfo);
                }
            }
        }
    }
}
