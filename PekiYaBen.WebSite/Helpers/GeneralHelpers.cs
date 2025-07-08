using PekiYaBen.WebSite.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PekiYaBen.WebSite.Helpers
{
    public static class GeneralHelpers
    {

        public static List<Product> GetProductList()
        {
            List<Product> products = new List<Product>();
            using (PekiYaBenDBEntities entities = new PekiYaBenDBEntities())
            {
                var cache = System.Web.HttpContext.Current.Cache;

                if (cache.Get("Products") == null)
                {

                    entities.Configuration.LazyLoadingEnabled = false;
                    products = entities.Products.SqlQuery(
                                                            "Select [Id],[Code],[Type],[Title],[Description], [Image],'' as [File], " +
                                                            "[ContentImage],'' as [ContentDescription],[Price],[SortOrder],[ShowMainPage]," +
                                                            "[Status] FROM Product Where Type=3 And Status=1 AND ShowMainPage=1 Order " +
                                                            "by SortOrder ASC").ToList();
                    cache.Insert("Products", products, null, DateTime.MaxValue, new TimeSpan(0, 10, 0));
                }
                else
                    products = (List<Product>)cache.Get("Products");

                return products;
            }
        }

        public static List<Content> GetContentList()
        {
            using (PekiYaBenDBEntities entities = new PekiYaBenDBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Contents.SqlQuery(
               $"Select [Id],[CategoryId],[CoachId],[Title],[Summary],Image, '' as Detail, [Type],[ProductIds],[MetaDescription],[MetaKeywords],[CreateDate],[Status] FROM Content Where Status=1 And Image Not Like '%data:%'")
               .ToList();
            }
        }

    }
}