using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetProductInfoQuery : IQuery
    {
        public GetProductInfoQuery(int id)
        {
            this.Id = id;
        }
        public int Id { get; set; }
    }
}
