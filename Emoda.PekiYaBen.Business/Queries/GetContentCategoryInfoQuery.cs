using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetContentCategoryInfoQuery : IQuery
    {
        public GetContentCategoryInfoQuery(int id)
        {
            this.Id = id;
        }
        public int Id { get; set; }
    }
}
