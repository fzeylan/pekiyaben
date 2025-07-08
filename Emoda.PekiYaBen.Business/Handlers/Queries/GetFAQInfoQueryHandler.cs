using Dapper;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Comment;
using Emoda.PekiYaBen.Entity.FAQ;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetFAQInfoQueryHandler : IQueryHandler<GetFAQInfoQuery, QuerySingleResult<FAQInfo>>
    {
        public QuerySingleResult<FAQInfo> Retrieve(GetFAQInfoQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.Query<FAQInfo>("SELECT * FROM FAQ WHERE Id = @Id", query, trans).SingleOrDefault();

                    return new QuerySingleResult<FAQInfo>(userInfo);
                }
            }
        }
    }
}
