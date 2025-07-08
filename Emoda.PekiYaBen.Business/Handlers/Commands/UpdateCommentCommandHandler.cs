using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity.Comment;
using OLCA.Infrastructure.CQS;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class UpdateCommentCommandHandler : ICommandHandler<UpdateCommentCommand, CommentInfo>
    {
        public CommentInfo Execute(UpdateCommentCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = con.Query<CommentInfo>("SELECT * FROM Comment WHERE Id=@Id", command, trans).SingleOrDefault();

                    if (existing != null)
                    {
                        existing.CoachId = command.CoachId;
                        existing.ProductId = command.ProductId;
                        existing.Title = command.Title;
                        existing.FullName = command.FullName;
                        existing.Comment = command.Comment;
                        existing.Stars = command.Stars;
                        existing.CreatedDate = command.CreatedDate;

                        con.Execute(@"UPDATE Comment 
                                     SET CoachId = @CoachId,    
                                         ProductId = @ProductId,
                                         Title = @Title,
                                         FullName = @FullName,
                                         Comment = @Comment,
                                         Stars = @Stars,
                                         CreatedDate = @CreatedDate
                                   WHERE Id=@Id", existing, trans);
                        trans.Commit();

                        return existing;
                    }
                    else
                    {
                        existing = new CommentInfo
                        {
                            CoachId = command.CoachId,
                            ProductId = command.ProductId,
                            FullName = command.FullName,
                            Title = command.Title,
                            Comment = command.Comment,
                            Stars = command.Stars,
                            CreatedDate = command.CreatedDate
                        };

                        var id = con.Query<int>("INSERT INTO Comment (CoachId, ProductId, FullName, Title, Comment, Stars, CreatedDate) VALUES(@CoachId, @ProductId, @FullName, @Title, @Comment, @Stars, @CreatedDate); SELECT CAST(SCOPE_IDENTITY() as int)", existing, trans).Single();
                        existing.Id = id;
                        trans.Commit();

                        return existing;
                    }
                }
            }
        }
    }
}
