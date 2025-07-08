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
    public class UpdateCoachCalendarCommandHandler : ICommandHandler<UpdateCoachCalendarCommand, int>
    {
        public int Execute(UpdateCoachCalendarCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = new CoachCalendarInfo
                    {
                        CoachId = command.CoachId,
                        AvailableDate = command.AvailableDate,
                        InterviewType = command.InterviewType,
                        ParticipantLimit = command.ParticipantLimit,
                        CoachIsFull = command.CoachIsFull,
                        Status = command.Status,
                    };

                    var id = con.Query<int>("INSERT INTO CoachCalendar (CoachId, AvailableDate, InterviewType, ParticipantLimit, CoachIsFull, Status) VALUES(@CoachId, @AvailableDate, @InterviewType, @ParticipantLimit, @CoachIsFull, @Status); SELECT CAST(SCOPE_IDENTITY() as int)", existing, trans).Single();
                    existing.Id = id;
                    trans.Commit();

                    return id;
                }
            }
        }
    }
}