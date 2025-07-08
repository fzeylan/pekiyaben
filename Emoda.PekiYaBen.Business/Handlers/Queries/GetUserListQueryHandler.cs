using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using Dapper;
using System.Text;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetUserListQueryHandler : IQueryHandler<GetUserListQuery, QueryListResult<UserListItem>>
    {
        private string _SEARCH_QUERY = @"
            WITH TempResult AS(SELECT Id, 
                   FullName, 
                   Email, 
                   ProfilePhoto,
                   DateofBirth, 
                   Gender,
                   CommunicationPermission, 
                   CommunicationPermissionUpdate,
                   SocialMedia,
                   PhoneNumber, 
                   Occupation, 
                   City, 
                   OccupationStatus,
                   Status,
                   RegisterDate,
                   LastLoginDate,
                   FCMToken,
                   (SELECT MAX(TransactionDate) FROM [Transaction] WHERE AppUserId = AppUser.Id AND Status>0) AS LastPurchaseDate
              FROM AppUser
             WHERE 1 = 1
               %s%), TempCount AS (SELECT COUNT(*) as RecordCount FROM AppUser WHERE 1=1 %s% )
          SELECT *
            FROM TempResult, TempCount
             %o%
          OFFSET @PageSize * (@PageNumber - 1) ROWS
      FETCH NEXT @PageSize ROWS ONLY";


        public QueryListResult<UserListItem> Retrieve(GetUserListQuery query)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                if (query.PageSize == 0)
                {
                    query.PageSize = 10;
                }

                if (query.PageNumber == 0)
                {
                    query.PageNumber = 1;
                }

                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(query.Search))
                {
                    sb.Append(@"AND (FullName LIKE '%' + @Search + '%' OR
                                 Email LIKE '%' + @Search + '%' OR
                                 PhoneNumber LIKE '%' + @Search + '%' OR
                                 Occupation LIKE '%' + @Search + '%' OR
                                 City LIKE '%' + @Search + '%')");
                }

                string queryStr = _SEARCH_QUERY.Replace("%s%", sb.ToString()).Replace("%o%", query.OrderBy);

                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.Query<UserListItem>(queryStr, query, trans);

                    return new QueryListResult<UserListItem>(userInfo);
                }
            }
        }
    }
}
