using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, int>
    {
        public int Execute(RegisterUserCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    UserInfo user = new UserInfo()
                    {
                        BirthDate = DateTime.ParseExact(command.BirthDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Email = command.Email,
                        NameSurname = command.NameSurname,
                        ComPermission = true,
                        ComLastDate = DateTime.Now,
                        Password = command.Password
                    };

                    var existing = con.QueryFirstOrDefault<int?>("SELECT 1 FROM UserInfo WHERE Email=@Email", new { Email = user.Email }, trans);
                    if (existing > 0)
                    {
                        throw new EmodaException("Bu email daha önce kaydolmuş");
                    }

                    var id = con.Query<int>("INSERT INTO UserInfo (NameSurname, Email, BirthDate, ComPermission,ComLastDate,Password) VALUES(@NameSurname, @Email, @BirthDate, @ComPermission, @ComLastDate, @Password); SELECT CAST(SCOPE_IDENTITY() as int)", user, trans).Single();
                    trans.Commit();
                    return id;
                }
            }
        }
    }

}
