using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using Dapper;
using System.Linq;
using Emoda.PekiYaBen.Entity.Coaching;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetCoachInfoQueryHandler : IQueryHandler<GetCoachInfoQuery, QuerySingleResult<CoachInfo>>
    {
        public QuerySingleResult<CoachInfo> Retrieve(GetCoachInfoQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.Query<CoachInfo>("SELECT * FROM Coach WHERE Id = @Id", query, trans).SingleOrDefault();

                    return new QuerySingleResult<CoachInfo>(userInfo);
                }
            }
        }
    }
}
