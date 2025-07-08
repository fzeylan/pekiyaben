using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetFAQListQuery : IQuery
    {
        public int PageNumber { get; set; } = 0;

        public int PageSize { get; set; } = 1000;

        public string Search { get; set; }

        public string OrderBy { get; set; }
    }
}
