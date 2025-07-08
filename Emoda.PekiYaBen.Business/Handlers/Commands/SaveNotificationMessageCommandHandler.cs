using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class SaveNotificationMessageCommandHandler : ICommandHandler<SaveNotificationMessageCommand, int>
    {
        public int Execute(SaveNotificationMessageCommand command)
        {
            bool allUsers = command.UserInfoId == null || command.UserInfoId.Length == 0;
            //using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            //{
            //    con.Open();
            //    using (IDbTransaction trans = con.BeginTransaction())
            //    {
            //        NotificationMessage message = new NotificationMessage
            //        {
            //            AllUsers = allUsers,
            //            DueDate = command.DueDate,
            //            IsDeleted = false,
            //            IsSent = false,
            //            Content = command.Content,
            //            Message = command.Message,
            //            SaveDate = DateTime.Now,
            //            SentDate = null,
            //            Title = command.Title
            //        };
            //        int id = con.Query<int>("INSERT INTO NotificationMessage (Title,Message, Content, DueDate, AllUsers, IsDeleted, IsSent, SaveDate, SentDate) VALUES(@Title, @Message,@Content, @DueDate, @AllUsers, @IsDeleted, @IsSent, @SaveDate, @SentDate); SELECT CAST(SCOPE_IDENTITY() as int)", message, trans).Single();
            //        if (!allUsers)
            //        {
            //            IEnumerable<NotificationParticipant> participants = from x in command.UserInfoId
            //                                                                select new NotificationParticipant
            //                                                                {
            //                                                                    IsDeleted = false,
            //                                                                    IsSent = false,
            //                                                                    NotificationId = id,
            //                                                                    SentDate = null,
            //                                                                    UserInfoId = x
            //                                                                };
            //            con.Execute("INSERT INTO NotificationParticipant(NotificationId, UserInfoId, IsDeleted, IsSent, SentDate) VALUES (@NotificationId, @UserInfoId, @IsDeleted, @IsSent, @SentDate)", participants, trans);
            //        }
            //        trans.Commit();
            //        return id;
            //    }
            //}
            return 0;
        }
    }
}
