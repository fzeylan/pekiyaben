using Dapper;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetUserPaymentsListQueryHandler : IQueryHandler<GetUserPaymentsListQuery, QueryListResult<PaymentItem>>
    {
        private const string _SEARCH_QUERY = @"SELECT p.Code, p.Title, t.* FROM [Transaction] t INNER JOIN Product p ON p.Id = t.ProductId WHERE AppUserId = @UserId AND t.Status>0";

        public QueryListResult<PaymentItem> Retrieve(GetUserPaymentsListQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();

                using (var trans = con.BeginTransaction())
                {
                    var payments = con.Query<PaymentItem>(_SEARCH_QUERY, query, trans);

                    return new QueryListResult<PaymentItem>(payments);
                }
            }
        }
    }
}
