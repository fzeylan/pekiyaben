using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetCoachListQuery : IQuery
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string Search { get; set; }

        public string OrderBy { get; set; }
    }
}
