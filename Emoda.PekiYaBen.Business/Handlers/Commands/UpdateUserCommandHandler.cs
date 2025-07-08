using Dapper;
using Dapper.Contrib.Extensions;
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
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, int>
    {
        public int Execute(UpdateUserCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = con.Query<UserInfo>("SELECT * FROM UserInfo WHERE Id=@Id", command, trans).SingleOrDefault();

                    if(existing==null)
                    {
                        throw new EmodaException("Kullanıcı bulunamadı");
                    }

                    existing.BirthDate = DateTime.ParseExact(command.BirthDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    //existing.Email = command.Email;
                    existing.NameSurname = command.NameSurname;
                    existing.Job = command.Job;
                    existing.Sex = command.Sex;
                    if(existing.ComPermission!= command.ComPermission)
                    {
                        existing.ComPermission = command.ComPermission;
                        existing.ComLastDate = DateTime.Now;
                    }
                    existing.CellPhone = command.Cellphone;
                    existing.Password = string.IsNullOrEmpty(command.Password) ? existing.Password : command.Password;
                    existing.WorkStatus = command.WorkStatus;
                    existing.City = command.City;
                    /*if(existing.Count>0)
                    {
                        throw new EmodaException("Bu email daha önce kaydolmuş");
                    }*/

                    con.Execute(@"UPDATE UserInfo 
                                     SET BirthDate = @BirthDate,    
                                         NameSurname = @NameSurname,
                                         Job = @Job,
                                         Sex = @Sex,
                                         ComPermission = @ComPermission,
                                         ComLastDate = @ComLastDate,
                                         CellPhone = @CellPhone,
                                         Password = @Password,
                                         WorkStatus = @WorkStatus,
                                         City = @City
                                   WHERE Id=@Id",existing, trans);
                    trans.Commit();
                    return 1;
                }
            }
        }
    }
}
