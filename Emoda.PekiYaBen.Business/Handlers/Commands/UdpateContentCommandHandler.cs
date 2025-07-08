using Dapper;
using Dapper.Contrib.Extensions;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity;
using Emoda.PekiYaBen.Entity.Coaching;
using Emoda.PekiYaBen.Entity.Content;
using Emoda.PekiYaBen.Entity.User;
using OLCA.Infrastructure.CQS;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class UdpateContentCommandHandler : ICommandHandler<UpdateContentCommand, ContentInfo>
    {
        public ContentInfo Execute(UpdateContentCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = con.Query<ContentInfo>("SELECT * FROM Content WHERE Id=@Id", command, trans).SingleOrDefault();

                    if (existing != null)
                    {
                        existing.Id = command.Id;
                        existing.CategoryId = command.CategoryId;
                        existing.CoachId = command.CoachId;
                        existing.Title = command.Title;
                        existing.Type = command.Type;
                        if (command.Image != null) existing.Image = command.Image;
                        existing.Summary = command.Summary;
                        existing.MetaKeywords = command.MetaKeywords;
                        existing.MetaDescription = command.MetaDescription;
                        existing.Detail = command.Detail;
                        existing.Status = command.Status;
                        existing.ProductIds = command.ProductIds;

                        con.Execute(@"UPDATE Content 
                                     SET CategoryId = @CategoryId,
                                            CoachId = @CoachId,
                                         Title = @Title,
                                         Type = @Type,
                                         Image = @Image,
                                         Summary = @Summary,
                                         Detail = @Detail,
                                         MetaDescription = @MetaDescription,
                                         MetaKeywords = @MetaKeywords,
                                         Status = @Status,
                                        ProductIds = @ProductIds
                                   WHERE Id=@Id", existing, trans);
                        trans.Commit();

                        return existing;
                    }
                    else
                    {
                        existing = new ContentInfo
                        {
                            CategoryId = command.CategoryId,
                            CoachId = command.CoachId,
                            Title = command.Title,
                            Type = command.Type,
                            Image = command.Image,
                            Summary = command.Summary,
                            Detail = command.Detail,
                            MetaDescription = command.MetaDescription,
                            MetaKeywords = command.MetaKeywords,
                            CreateDate = DateTime.Now,
                            Status = command.Status,
                            ProductIds  = command.ProductIds
                        };
                        
                        var id = con.Query<int>("INSERT INTO Content (CategoryId, CoachId, Title, Type, Image, Summary, Detail, MetaDescription, MetaKeywords, CreateDate, Status, ProductIds) VALUES(@CategoryId, @CoachId, @Title, @Type, @Image, @Summary, @Detail, @MetaDescription, @MetaKeywords, @CreateDate, @Status, @ProductIds); SELECT CAST(SCOPE_IDENTITY() as int)", existing, trans).Single();
                        existing.Id = id;
                        trans.Commit();

                        return existing;
                    }
                }
            }
        }
    }
}
