using Microsoft.Extensions.Options;
using PekiYaBen.API.Commands;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Models.APIModels;

namespace PekiYaBen.API.Services
{
    public interface IContentService
    {
        APIResponse GetAnalyzeTitles(string tokenValue, int parentId);
        APIResponse GetValues(string tokenValue);
        APIResponse GetCoachingList(string tokenValue);
        APIResponse GetAudioFiles(string tokenValue);
        APIResponse SetPayment(string tokenValue, Payment payment);
        APIResponse GetUserProduct(string tokenValue, string productCode);
        APIResponse SetInterview(string tokenValue, UserInterview interview);
        APIResponse WheelOfLife(string tokenValue, WheelOfLife wheel);
        APIResponse PersonalStrategy(string tokenValue, PersonalStrategy strategy);
        APIResponse GetCoaches(string tokenValue, string product);
        APIResponse GetCoachCalendar(string tokenValue, CoachDates request);
        APIResponse SetCoachInterview(string tokenValue, CoachInterview interview);
        APIResponse GetUserCoaching(string tokenValue);
        APIResponse UpdateCoaching(string tokenValue, PostponeCoaching interview);
        APIResponse CancelCoaching(string tokenValue, CancelCoaching interview);
    }

    public class ContentService : IContentService
    {
        private readonly AppSettings _appSettings;

        public ContentService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public APIResponse GetAnalyzeTitles(string token, int parentId)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetAnalyzeTitles(token, parentId);

            return new APIResponse(response);
        }

        public APIResponse GetValues(string tokenValue)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetValues(tokenValue);

            return new APIResponse(response);
        }

        public APIResponse GetCoachingList(string tokenValue)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetCoachingList(tokenValue);

            return new APIResponse(response);
        }

        public APIResponse GetAudioFiles(string token)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetAudioFiles(token);

            return new APIResponse(response);
        }

        public APIResponse SetPayment(string tokenValue, Payment payment)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.SetPayment(tokenValue, payment);

            return new APIResponse(response);
        }

        public APIResponse GetUserProduct(string tokenValue, string productCode)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetUserProduct(tokenValue, productCode);

            return new APIResponse(response);
        }

        public APIResponse SetInterview(string tokenValue, UserInterview interview)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.SetInterview(tokenValue, interview);

            return new APIResponse(response);
        }

        public APIResponse WheelOfLife(string tokenValue, WheelOfLife wheel)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.WheelOfLife(tokenValue, wheel);

            return new APIResponse(response);
        }

        public APIResponse PersonalStrategy(string tokenValue, PersonalStrategy strategy)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.PersonalStrategy(tokenValue, strategy);

            return new APIResponse(response);
        }

        public APIResponse GetCoaches(string tokenValue, string product)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetCoaches(tokenValue, product);

            return new APIResponse(response);
        }

        public APIResponse GetCoachCalendar(string tokenValue, CoachDates request)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetCoachCalendar(tokenValue, request);

            return new APIResponse(response);
        }

        public APIResponse GetUserCoaching(string tokenValue)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.GetUserCoaching(tokenValue);

            return new APIResponse(response);
        }

        public APIResponse SetCoachInterview(string tokenValue, CoachInterview interview)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.SetCoachInterview(tokenValue, interview);

            return new APIResponse(response);
        }

        public APIResponse UpdateCoaching(string tokenValue, PostponeCoaching interview)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.UpdateCoaching(tokenValue, interview);

            return new APIResponse(response);
        }

        public APIResponse CancelCoaching(string tokenValue, CancelCoaching interview)
        {
            ContentCommand command = new ContentCommand(_appSettings);
            CommandResponse response = command.CancelCoaching(tokenValue, interview);

            return new APIResponse(response);
        }
    }
}