using System;
using System.ComponentModel.DataAnnotations;

namespace PekiYaBen.API.Models.APIModels
{
    public class CoachInterview
    {

        [Required]
        public int CalendarId{ get; set; }
        [Required]
        public string InterviewType { get; set; }
    }
}
