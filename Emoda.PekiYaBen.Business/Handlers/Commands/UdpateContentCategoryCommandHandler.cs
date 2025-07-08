using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity.Content;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class UdpateContentCategoryCommandHandler : ICommandHandler<UpdateContentCategoryCommand, ContentCategoryInfo>
    {
        public ContentCategoryInfo Execute(UpdateContentCategoryCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = con.Query<ContentCategoryInfo>("SELECT * FROM ContentCategory WHERE Id=@Id", command, trans).SingleOrDefault();

                    if (existing != null)
                    {
                        existing.Id = command.Id;
                        existing.Title = command.Title;
                        existing.Type = command.Type;
                        existing.Status = command.Status;

                        con.Execute(@"UPDATE ContentCategory 
                                     SET Title = @Title,
                                         Type = @Type,
                                         Status = @Status
                                   WHERE Id=@Id", existing, trans);
                        trans.Commit();

                        return existing;
                    }
                    else
                    {
                        existing = new ContentCategoryInfo
                        {
                            Title = command.Title,
                            Type = command.Type,
                            Status = command.Status,
                        };

                        
                        var id = con.Query<int>("INSERT INTO ContentCategory (Title, Type, Status) VALUES(@Title, @Type, @Status); SELECT CAST(SCOPE_IDENTITY() as int)", existing, trans).Single();
                        existing.Id = id;
                        trans.Commit();

                        return existing;
                    }
                }
            }
        }
    }
}
