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
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductInfo>
    {
        public ProductInfo Execute(UpdateProductCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    var existing = con.Query<ProductInfo>("SELECT * FROM Product WHERE Id=@Id", command, trans).SingleOrDefault();

                    if (existing != null)
                    {
                        existing.Id = command.Id;
                        existing.Code = command.Code;
                        existing.Title = command.Title;
                        existing.Type = command.Type;
                        existing.Description = command.Description;
                        existing.Image= command.Image;
                        existing.ContentDescription = command.ContentDescription;
                        existing.ContentImage= command.ContentImage;
                        existing.File = command.File;
                        existing.Price = command.Price;
                        existing.SortOrder = command.SortOrder;
                        existing.Status = command.Status;
                        existing.ShowMainPage = command.ShowMainPage;

                        con.Execute(@"UPDATE Product 
                                     SET Code = @Code,    
                                         Title = @Title,
                                         Type = @Type,
                                         Description = @Description,
                                         Image = @Image,
                                         ContentDescription = @ContentDescription,
                                         ContentImage = @ContentImage,
                                         [File] = @File,
                                         Price = @Price,
                                         SortOrder = @SortOrder,
                                         Status = @Status,
                                         ShowMainPage = @ShowMainPage

                                   WHERE Id=@Id", existing, trans);
                        trans.Commit();

                        return existing;
                    }
                    else
                    {
                        existing = new ProductInfo
                        {
                            Code = command.Code,
                            Title = command.Title,
                            Type = command.Type,
                            Description = command.Description,
                            Image = command.Image,
                            ContentDescription = command.ContentDescription,
                            ContentImage = command.ContentImage,
                            File = command.File,
                            Price = command.Price,
                            SortOrder = command.SortOrder,
                            Status = command.Status,
                            ShowMainPage = command.ShowMainPage
                        };

                        var id = con.Query<int>("INSERT INTO Product (Code, Type, Title, Description, Image, [File], ContentImage, ContentDescription, Price, SortOrder, Status,ShowMainPage) VALUES(@Code, @Type, @Title, @Description, @Image, @File, @ContentImage, @ContentDescription, @Price, @SortOrder, @Status, @ShowMainPage); SELECT CAST(SCOPE_IDENTITY() as int)", existing, trans).Single();
                        existing.Id = id;
                        trans.Commit();

                        return existing;
                    }
                }
            }
        }
    }
}
