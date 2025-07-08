using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity.FAQ;
using OLCA.Infrastructure.CQS;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class UpdateFAQCommandHandler : ICommandHandler<UpdateFAQCommand, FAQInfo>
    {
        public FAQInfo Execute(UpdateFAQCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = con.Query<FAQInfo>("SELECT * FROM FAQ WHERE Id=@Id", command, trans).SingleOrDefault();

                    if (existing != null)
                    {
                        existing.SortOrder = command.SortOrder;
                        existing.Title = command.Title;
                        existing.Description = command.Description;
                        existing.Status = command.Status;
                      
                        con.Execute(@"UPDATE FAQ 
                                     SET SortOrder = @SortOrder,    
                                         Title = @Title,
                                         Description = @Description,
                                         Status = @Status
                                   WHERE Id=@Id", existing, trans);
                        trans.Commit();

                        return existing;
                    }
                    else
                    {
                        existing = new FAQInfo
                        {
                            SortOrder = command.SortOrder,
                            Title = command.Title,
                            Description = command.Description,
                            Status = command.Status,
                            CreatedDate = DateTime.Now
                        };

                        var id = con.Query<int>("INSERT INTO FAQ (SortOrder, Title, Description, Status, CreatedDate) VALUES(@SortOrder, @Title, @Description, @Status, @CreatedDate); SELECT CAST(SCOPE_IDENTITY() as int)", existing, trans).Single();
                        existing.Id = id;
                        trans.Commit();

                        return existing;
                    }
                }
            }
        }
    }
}
