using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetUserInfoQuery : IQuery
    {
        public GetUserInfoQuery(int id)
        {
            this.Id = id;
        }
        public int Id { get; set; }
    }
}
