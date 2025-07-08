using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetUserPaymentsListQuery : IQuery
    {
        public int UserId { get; set; }
    }
}
