using Newtonsoft.Json;
using PekiYaBen.WebSite.Extensions;
using PekiYaBen.WebSite.Helpers;
using PekiYaBen.WebSite.Models;
using PekiYaBen.WebSite.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Routing;

namespace PekiYaBen.WebSite.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WebApiController : Controller
    {
        private readonly PekiYaBenDBEntities entities = new PekiYaBenDBEntities();

        [HttpGet]
        [Route("webapi/homepage")]

        public async Task<JsonResult> GetHomepageData()
        {
            try
            {
                var response = new
                {
                    coaches = GetFeaturedCoaches(),
                    products = GetFeaturedProducts(),
                    latestContent = GetLatestContent(),
                    statistics = GetStatistics(),
                    heroContent = GetHeroContent(),
                    success = true
                };

                return Json(new ApiResponse(response), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse("An error occurred while fetching homepage data: " + ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("webapi/coaches")]
        public async Task<JsonResult> GetFeaturedCoaches()
        {
            try
            {
                var coaches = entities.Coaches
                    .Where(x => x.ShowMainPage == true && x.Status == 1)
                    .OrderByDescending(x => x.Status)
                    .ThenBy(x => x.FullName)
                    .Select(c => new
                    {
                        id = c.Id,
                        name = c.FullName,
                        title = c.Title,
                        description = c.Description,
                        image = c.ProfilePhoto,
                        //slug = c.FullName.FriendlyUrl(),
                        categoryId = 1, // Default category for routing
                        status = c.Status
                    })
                    .Take(6) // Limit to 6 coaches for homepage
                    .ToList();

                return Json(new ApiResponse(coaches), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse("An error occurred while fetching coaches: " + ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("webapi/products")]
        public async Task<ApiResponse> GetFeaturedProducts()
        {
            try
            {
                var products = GeneralHelpers.GetProductList()
                    .Where(p => p.ShowMainPage == true && p.Status == 1)
                    .OrderBy(p => p.SortOrder)
                    .Select(p => new
                    {
                        id = p.Id,
                        code = p.Code,
                        title = p.Title,
                        description = p.Description,
                        image = p.Image,
                        contentImage = p.ContentImage,
                        price = p.Price,
                        slug = p.Title.FriendlyUrl(),
                        sortOrder = p.SortOrder
                    })
                    .Take(4) // Limit to 4 products for homepage
                    .ToList();

                return new ApiResponse(products);
            }
            catch (Exception ex)
            {
                return new ApiResponse(new Exception("An error occurred while fetching products: " + ex.Message));
            }
        }

        [HttpGet]
        [Route("latest-content")]
        public async Task<ApiResponse> GetLatestContent()
        {
            try
            {
                var latestContent = GeneralHelpers.GetContentList()
                    .Where(c => c.Status == 1)
                    .OrderByDescending(c => c.CreateDate)
                    .Take(3) // Get latest 3 content items
                    .Select(c => new
                    {
                        id = c.Id,
                        categoryId = c.CategoryId,
                        coachId = c.CoachId,
                        title = c.Title,
                        summary = c.Summary,
                        image = c.Image,
                        type = c.Type,
                        createDate = c.CreateDate,
                        slug = c.Title.FriendlyUrl(),
                        category = GetCategoryInfo(c.CategoryId),
                        coach = GetCoachInfo(c.CoachId)
                    })
                    .ToList();

                return new ApiResponse(latestContent);
            }
            catch (Exception ex)
            {
                return new ApiResponse(new Exception("An error occurred while fetching latest content: " + ex.Message));
            }
        }

        [HttpGet]
        [Route("statistics")]
        public async Task<ApiResponse> GetStatistics()
        {
            try
            {
                var parameters = entities.GeneralParameters.ToList();

                var statistics = new
                {
                    coachingHours = GetParameterValue(parameters, "KoclukSaati") ?? "1000+",
                    users = GetParameterValue(parameters, "Kullanici") ?? "5000+",
                    workshopHours = GetParameterValue(parameters, "SaatAtolye") ?? "500+",
                    countries = GetParameterValue(parameters, "Ulke") ?? "15+",
                    totalCoaches = entities.Coaches.Count(c => c.Status == 1),
                    activeUsers = entities.AppUsers.Count(u => u.Status == true),
                    totalContent = entities.Contents.Count(c => c.Status == 1)
                };

                return new ApiResponse(statistics);
            }
            catch (Exception ex)
            {
                return new ApiResponse(new Exception("An error occurred while fetching statistics: " + ex.Message));
            }
        }

        [HttpGet]
        [Route("hero-content")]
        public async Task<ApiResponse> GetHeroContent()
        {
            try
            {
                var parameters = entities.GeneralParameters.ToList();

                var heroContent = new
                {
                    mainPageVideo = GetParameterValue(parameters, "MainPageVideo"),
                    title = GetParameterValue(parameters, "AnaSayfaBaslik") ?? "PekiYaBen ile Hayatını Değiştir",
                    subtitle = GetParameterValue(parameters, "AnaSayfaAltAciklama") ?? "Uzman koçlarımızla birebir görüşmeler yapın ve kişisel gelişiminizi hızlandırın."
                };

                return new ApiResponse(heroContent);
            }
            catch (Exception ex)
            {
                return new ApiResponse(new Exception("An error occurred while fetching hero content: " + ex.Message));
            }
        }

        [HttpGet]
        [Route("testimonials")]
        public async Task<ApiResponse> GetTestimonials()
        {
            try
            {
                var testimonials = entities.Comments
                    .Where(c => c.Stars >= 4) // Only show good reviews
                    .OrderByDescending(c => c.CreatedDate)
                    .Select(c => new
                    {
                        id = c.Id,
                        fullName = c.FullName,
                        title = c.Title,
                        comment = c.Comment1,
                        stars = c.Stars,
                        createdDate = c.CreatedDate,
                        coachId = c.CoachId,
                        productId = c.ProductId
                    })
                    .Take(6) // Limit to 6 testimonials
                    .ToList();

                return new ApiResponse(testimonials);
            }
            catch (Exception ex)
            {
                return new ApiResponse(new Exception("An error occurred while fetching testimonials: " + ex.Message));
            }
        }

        [HttpGet]
        [Route("faqs")]
        public async Task<ApiResponse> GetFrequentlyAskedQuestions()
        {
            try
            {
                var faqs = entities.FAQs
                    .OrderBy(f => f.SortOrder)
                    .Select(f => new
                    {
                        id = f.Id,
                        question = f.Title,
                        answer = f.Description,
                        sortOrder = f.SortOrder
                    })
                    .Take(8) // Limit to 8 FAQs for homepage
                    .ToList();

                return new ApiResponse(faqs);
            }
            catch (Exception ex)
            {
                return new ApiResponse(new Exception("An error occurred while fetching FAQs: " + ex.Message));
            }
        }

        [HttpGet]
        [Route("content-categories")]
        public async Task<ApiResponse> GetContentCategories()
        {
            try
            {
                var categories = entities.ContentCategories
                    .Where(c => c.Status == 1)
                    .OrderBy(c => c.Title)
                    .Select(c => new
                    {
                        id = c.Id,
                        title = c.Title,
                        type = c.Type,
                        slug = c.Title.FriendlyUrl(),
                        contentCount = entities.Contents.Count(content => content.CategoryId == c.Id && content.Status == 1)
                    })
                    .ToList();

                return new ApiResponse(categories);
            }
            catch (Exception ex)
            {
                return new ApiResponse(new Exception("An error occurred while fetching content categories: " + ex.Message));
            }
        }

        #region Private Helper Methods

        private object GetCategoryInfo(int? categoryId)
        {
            if (!categoryId.HasValue) return null;

            var category = entities.ContentCategories.FirstOrDefault(c => c.Id == categoryId.Value);
            if (category == null) return null;

            return new
            {
                id = category.Id,
                title = category.Title,
                type = category.Type,
                slug = category.Title.FriendlyUrl()
            };
        }

        private object GetCoachInfo(int? coachId)
        {
            if (!coachId.HasValue) return null;

            var coach = entities.Coaches.FirstOrDefault(c => c.Id == coachId.Value);
            if (coach == null) return null;

            return new
            {
                id = coach.Id,
                fullName = coach.FullName,
                title = coach.Title,
                profilePhoto = coach.ProfilePhoto,
                slug = coach.FullName.FriendlyUrl()
            };
        }

        private string GetParameterValue(List<GeneralParameter> parameters, string parameterName)
        {
            return parameters.FirstOrDefault(p => p.Parameter == parameterName)?.Value;
        }

        #endregion
    }
}