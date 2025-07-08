using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class RenewPasswordCommandHandler : ICommandHandler<RenewPasswordCommand, string>
    {
        RNGCryptoServiceProvider rProvider = new RNGCryptoServiceProvider();
        public string Execute(RenewPasswordCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var userInfo = con.QueryFirstOrDefault<AdminUserInfo>("SELECT * FROM AdminUser WHERE Email=@Email", command, trans);

                    if (userInfo == null)
                    {
                        return string.Empty;
                    }

                    string password = CreatePassword(8);
                    int rows = con.Execute("UPDATE AdminUser SET Password=@Password WHERE Email= @Email", new { Password = password, Email = command.Email }, trans);
                    trans.Commit();
                    return rows == 1 ? password : string.Empty;

                }
            }
        }

        private string CreatePassword(int length)
        {
            StringBuilder res = new StringBuilder();
            byte[] random = new byte[1];
            using (rProvider)
            {
                while (0 < length--)
                {
                    char rndChar = '\0';
                    do
                    {
                        rProvider.GetBytes(random);
                        rndChar = (char)((random[0] % 92) + 33);
                    } while (!char.IsLetterOrDigit(rndChar));
                    res.Append(rndChar);
                }
            }
            return res.ToString();
        }
    }
}
