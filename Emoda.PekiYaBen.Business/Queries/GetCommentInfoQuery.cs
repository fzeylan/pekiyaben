using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetCommentInfoQuery : IQuery
    {
        public GetCommentInfoQuery(int id)
        {
            this.Id = id;
        }
        public int Id { get; set; }
    }
}
