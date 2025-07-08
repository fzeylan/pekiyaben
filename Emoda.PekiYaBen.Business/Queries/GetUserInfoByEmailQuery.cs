using OLCA.Infrastructure.CQS;

namespace Emoda.PekiYaBen.Business.Queries
{
    public class GetUserInfoByEmailQuery : IQuery
    {
        public GetUserInfoByEmailQuery(string email)
        {
            this.Email = email;
        }
        public string Email { get; set; }
    }
}
