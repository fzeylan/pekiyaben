using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetFAQInfoQuery : IQuery
    {
        public GetFAQInfoQuery(int id)
        {
            this.Id = id;
        }
        public int Id { get; set; }
    }
}
