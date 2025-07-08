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
    public class UpdateCoachCommandHandler : ICommandHandler<UpdateCoachCommand, CoachInfo>
    {
        public CoachInfo Execute(UpdateCoachCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = con.Query<CoachInfo>("SELECT * FROM Coach WHERE Id=@Id", command, trans).SingleOrDefault();

                    if (existing != null)
                    {
                        existing.FullName = command.FullName;
                        existing.Email = command.Email;
                        existing.DateOfBirth = command.DateOfBirth;
                        existing.PhoneNumber = command.PhoneNumber;
                        existing.Title = command.Title;
                        existing.Description = command.Description;
                        existing.ProfilePhoto = command.ProfilePhoto;
                        //existing.Password = string.IsNullOrEmpty(command.Password) ? existing.Password : command.Password;
                        existing.EducationInfo = command.EducationInfo;
                        existing.CoachingInfo = command.CoachingInfo;
                        existing.CertificateInfo = command.CertificateInfo;
                        existing.Status = command.Status;
                        existing.ShowMainPage = command.ShowMainPage;

                        con.Execute(@"UPDATE Coach 
                                     SET FullName = @FullName,    
                                         Email = @Email,
                                         DateOfBirth = @DateOfBirth,
                                         PhoneNumber = @PhoneNumber,
                                         Title = @Title,
                                         Description = @Description,
                                         ProfilePhoto = @ProfilePhoto,
                                         EducationInfo = @EducationInfo,
                                         CoachingInfo = @CoachingInfo,
                                         CertificateInfo = @CertificateInfo,
                                         Status = @Status,
                                         ShowMainPage = @ShowMainPage
                                   WHERE Id=@Id", existing, trans);
                        trans.Commit();

                        return existing;
                    }
                    else
                    {
                        existing = new CoachInfo
                        {
                            FullName = command.FullName,
                            Email = command.Email,
                            DateOfBirth = command.DateOfBirth,
                            PhoneNumber = command.PhoneNumber,
                            Title = command.Title,
                            ProfilePhoto = command.ProfilePhoto,
                            Description = command.Description,
                            EducationInfo = command.EducationInfo,
                            CoachingInfo = command.CoachingInfo,
                            CertificateInfo = command.CertificateInfo,
                            Status = command.Status,
                            ShowMainPage = command.ShowMainPage
                        };

                        var id = con.Query<int>("INSERT INTO Coach (FullName, Email, DateOfBirth,PhoneNumber,Title, Description,ProfilePhoto,EducationInfo,CoachingInfo,CertificateInfo,Status, ShowMainPage) VALUES(@FullName, @Email, @DateOfBirth,@PhoneNumber,@Title, @Description,@ProfilePhoto, @EducationInfo,@CoachingInfo,@CertificateInfo,@Status,@ShowMainPage); SELECT CAST(SCOPE_IDENTITY() as int)", existing, trans).Single();
                        existing.Id = id;
                        trans.Commit();

                        return existing;
                    }
                }
            }
        }
    }
}
