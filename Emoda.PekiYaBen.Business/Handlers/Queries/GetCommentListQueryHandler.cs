using Dapper;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetCommentListQueryHandler : IQueryHandler<GetCommentListQuery, QueryListResult<CommentListItem>>
    {
        private string _SEARCH_QUERY = @"WITH TempResult AS(SELECT Comment.Id, Coach.FullName as CoachFullName, Product.Title as ProductTitle, Comment.FullName, Coach.Title, Comment.Comment, Comment.Stars, Comment.CreatedDate FROM Comment INNER JOIN Coach on Coach.Id = Comment.CoachId INNER JOIN Product on Product.Id = Comment.ProductId WHERE 1 = 1), TempCount AS (SELECT COUNT(*) as RecordCount FROM Comment WHERE 1=1) SELECT * FROM TempResult, TempCount ORDER BY CreatedDate desc";


        public QueryListResult<CommentListItem> Retrieve(GetCommentListQuery query)
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
                    var info = con.Query<CommentListItem>(queryStr, query, trans);

                    return new QueryListResult<CommentListItem>(info);
                }
            }
        }
    }
}
