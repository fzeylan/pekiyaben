using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Models;
using PekiYaBen.API.Models.APIModels;
using PekiYaBen.API.Models.Entities;
using PekiYaBen.API.Services;

namespace PekiYaBen.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContentController : ControllerBase
    {
        private IContentService _contentService;

        public ContentController(IContentService ContentService)
        {
            _contentService = ContentService;
        }

        [HttpPost("contentlist")]
        public IActionResult GetContentTitles(GetContent request)
        {

            var tokenValue = GetToken();
            var response = _contentService.GetAnalyzeTitles(tokenValue, request.ParentId);

            var logger = new SimpleLogger();
            logger.Info("Request : GetAnalyzeTitles " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : GetAnalyzeTitles " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpGet("coachinglist")]
        public IActionResult GetCoachingList()
        {
            var tokenValue = GetToken();
            var response = _contentService.GetCoachingList(tokenValue);

            var logger = new SimpleLogger();
            logger.Info("Request : GetCoachingList " + tokenValue);
            logger.Info("Response : GetCoachingList " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);

        }

        [HttpGet("valuelist")]
        public IActionResult GetValues()
        {
            var tokenValue = GetToken();
            var response = _contentService.GetValues(tokenValue);

            var logger = new SimpleLogger();
            logger.Info("Request : GetValues " + tokenValue);
            logger.Info("Response : GetValues " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);

        }

        [HttpPost("audiolist")]
        public IActionResult GetAudioFiles()
        {
            var tokenValue = GetToken();
            var response = _contentService.GetAudioFiles(tokenValue);

            var logger = new SimpleLogger();
            logger.Info("Request : GetAudioFiles " + tokenValue);
            logger.Info("Response : GetAudioFiles " + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("setpayment")]
        public IActionResult SetPayment(Payment request)
        {
            var tokenValue = GetToken();
            var response = _contentService.SetPayment(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : SetPayment " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : SetPayment " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);

        }

        [HttpPost("userproduct")]
        public IActionResult GetProducts(UserProduct request)
        {
            var tokenValue = GetToken();
            var response = _contentService.GetUserProduct(tokenValue, request.ProductCode);

            var logger = new SimpleLogger();
            logger.Info("Request : GetProducts " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : GetProducts " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("interview")]
        public IActionResult SetInterview(UserInterview request)
        {
            var tokenValue = GetToken();
            var response = _contentService.SetInterview(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : SetInterview " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : SetInterview " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("wheel")]
        public IActionResult SetWheel(WheelOfLife request)
        {
            var tokenValue = GetToken();
            var response = _contentService.WheelOfLife(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : SetWheel " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : SetWheel " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("strategy")]
        public IActionResult SetStrategy(PersonalStrategy request)
        {
            var tokenValue = GetToken();
            var response = _contentService.PersonalStrategy(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : SetStrategy " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : SetStrategy " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("coachlist")]
        public IActionResult GetCoaches(CoachListRequest coachListRequest)
        {
            var tokenValue = GetToken();
            var response = _contentService.GetCoaches(tokenValue, coachListRequest.product);

            var logger = new SimpleLogger();
            logger.Info("Request : GetCoaches " + coachListRequest.product);
            logger.Info("Response : GetCoaches " + coachListRequest.product);

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("coachcalendar")]
        public IActionResult GetCoachCalendar(CoachDates request)
        {
            var tokenValue = GetToken();
            var response = _contentService.GetCoachCalendar(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : GetCoachCalendar " + JsonConvert.SerializeObject(request));
            logger.Info("Response : GetCoachCalendar " + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }
        [HttpPost("coachinterview")]
        public IActionResult SetCoachInterview(CoachInterview request)
        {
            var tokenValue = GetToken();
            var response = _contentService.SetCoachInterview(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : SetCoachInterview " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : SetCoachInterview " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("userinterviews")]
        public IActionResult GetUserCoaching()
        {
            var tokenValue = GetToken();
            var response = _contentService.GetUserCoaching(tokenValue);

            var logger = new SimpleLogger();
            logger.Info("Request : GetUserCoaching " + tokenValue);
            logger.Info("Response : GetUserCoaching " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("postponeinterview")]
        public IActionResult PostponeUserCoaching(PostponeCoaching request)
        {
            var tokenValue = GetToken();
            var response = _contentService.UpdateCoaching(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : PostponeUserCoaching " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : PostponeUserCoaching " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        [HttpPost("cancelinterview")]
        public IActionResult CancelUserCoaching(CancelCoaching request)
        {
            var tokenValue = GetToken();
            var response = _contentService.CancelCoaching(tokenValue, request);

            var logger = new SimpleLogger();
            logger.Info("Request : CancelUserCoaching " + tokenValue + JsonConvert.SerializeObject(request));
            logger.Info("Response : CancelUserCoaching " + tokenValue + JsonConvert.SerializeObject(response));

            if (!String.IsNullOrEmpty(response.Message) && response.Message.Contains("401"))
                return StatusCode(401, "Unauthorized");

            return Ok(response);
        }

        private string GetToken()
        {
            StringValues token;
            HttpContext.Request.Headers.TryGetValue("Authorization", out token);
            return token.ToString().Split(' ')[1].ToString();
        }
    }
}
