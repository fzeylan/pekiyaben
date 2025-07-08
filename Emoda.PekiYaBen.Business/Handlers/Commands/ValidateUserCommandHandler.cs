using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class ValidateUserQueryHandler : ICommandHandler<ValidateUserCommand, AdminUserInfo>
    {
        public AdminUserInfo Execute(ValidateUserCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction()) 
                {
                    var userInfo = con.QueryFirstOrDefault<AdminUserInfo>("SELECT * FROM AdminUser WHERE Email=@Email", command, trans);

                    if (userInfo == null)
                    {
                        throw new EmodaException("Kullanıcı bulunamadı.");
                    }

                    if (userInfo.Password != command.Password)
                    {

                        throw new EmodaException("Yanlış kullanıcı adı veya şifre");
                    }

                    trans.Commit();

                    return userInfo;
                }
            }
        }
    }
}
