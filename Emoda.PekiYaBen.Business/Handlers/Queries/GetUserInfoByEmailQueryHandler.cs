using Dapper;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetUserInfoByEmailQueryHandler : IQueryHandler<GetUserInfoByEmailQuery, QuerySingleResult<AppUserInfo>>
    {
        public QuerySingleResult<AppUserInfo> Retrieve(GetUserInfoByEmailQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.Query<AppUserInfo>("SELECT * FROM AdminUser WHERE Email = @Email", query, trans).SingleOrDefault();

                    return new QuerySingleResult<AppUserInfo>(userInfo);
                }
            }
        }
    }
}
