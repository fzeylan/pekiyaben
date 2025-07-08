using Dapper;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Comment;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetCommentInfoQueryHandler : IQueryHandler<GetCommentInfoQuery, QuerySingleResult<CommentInfo>>
    {
        public QuerySingleResult<CommentInfo> Retrieve(GetCommentInfoQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.Query<CommentInfo>("SELECT * FROM Comment WHERE Id = @Id", query, trans).SingleOrDefault();

                    return new QuerySingleResult<CommentInfo>(userInfo);
                }
            }
        }
    }
}
