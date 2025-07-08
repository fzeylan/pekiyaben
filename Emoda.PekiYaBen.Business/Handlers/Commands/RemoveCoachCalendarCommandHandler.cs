using Dapper;
using Dapper.Contrib.Extensions;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity;
using Emoda.PekiYaBen.Entity.Coaching;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class RemoveCoachCalendarCommandHandler : ICommandHandler<RemoveCoachCalendarCommand, bool>
    {
        public bool Execute(RemoveCoachCalendarCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                try
                {
                    using (var trans = con.BeginTransaction())
                    {
                        var existing = new CoachCalendarInfo
                        {
                            Id = command.Id
                        };

                        con.Query("DELETE FROM CoachCalendar WHERE Id=@Id And Status=1 AND (Select count(*) from CoachCalendarUser ccu Where CoachCalendarId=@Id)=0", existing, trans);
                        trans.Commit();

                        return true;
                    }
                }
                catch (Exception ex)
                {
                }
                return false;
            }
        }
    }
}