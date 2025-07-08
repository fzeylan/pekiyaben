using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetContentInfoQuery : IQuery
    {
        public GetContentInfoQuery(int id)
        {
            this.Id = id;
        }
        public int Id { get; set; }
    }
}
