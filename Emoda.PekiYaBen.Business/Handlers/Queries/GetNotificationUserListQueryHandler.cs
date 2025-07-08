using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using Dapper;
using System.Text;

namespace Emoda.PekiYaBen.Business.Handlers.Queries
{
    public class GetNotificationUserListQueryHandler : IQueryHandler<GetNotificationUserListQuery, QueryListResult<UserListItem>>
    {
        private string _SEARCH_QUERY = @"
            WITH TempResult AS(SELECT Id, 
                   FullName, 
                   Email, 
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
                   (SELECT MAX(TransactionDate) FROM [Transaction] WHERE AppUserId = AppUser.Id ) AS LastPurchaseDate
              FROM AppUser
             WHERE 1 = 1 And (CommunicationPermission=1 OR ((SELECT Count(Id) FROM [Transaction] t WHERE t.AppUserId = AppUser.Id AND t.Status>0) > 0)) And FCMToken IS NOT NULL
               %s%), TempCount AS (SELECT COUNT(*) as RecordCount FROM AppUser WHERE 1=1 And (CommunicationPermission=1 OR ((SELECT Count(Id) FROM [Transaction] t WHERE t.AppUserId = AppUser.Id AND t.Status>0) > 0)) AND FCMToken IS NOT NULL %s% )
          SELECT *
            FROM TempResult, TempCount
             %o%
          OFFSET @PageSize * (@PageNumber - 1) ROWS
      FETCH NEXT @PageSize ROWS ONLY";


        public QueryListResult<UserListItem> Retrieve(GetNotificationUserListQuery query)
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
