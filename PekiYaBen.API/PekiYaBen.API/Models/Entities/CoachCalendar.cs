using PekiYaBen.API.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.Entities
{
    public class CoachCalendar
    {

        public int Id { get; set; }
        public int CoachId { get; set; }
        public string FullName { get; set; }
        public string InterviewType { get; set; }
        public string ProfilePhoto { get; set; }
        [Required]
        public DateTime InterviewDate { get; set; }

        public string InterviewDateStr
        {
            get
            {
                return InterviewDate.ToDayMonthYearHourMinute(false);
            }
        }

        public CoachInterviewStatus Status { get; set; }
        public string StatusColor
        {
            get
            {
                var color = "";
                if (Status == CoachInterviewStatus.Active)
                    color = "d05252";
                if (Status == CoachInterviewStatus.Booked)
                    color = "b3ae61";
                else if (Status == CoachInterviewStatus.Passive)
                    color = "75748a";
                else if (Status == CoachInterviewStatus.Completed)
                    color = "37a325";

                return color;
            }
        }

        public string StatusStr
        {
            get
            {
                return Status.GetDescription();
            }
        }


        public bool IsDeletable
        {
            get
            {
                return (Status == CoachInterviewStatus.Booked && InterviewDate > DateTime.Now.AddHours(24));
            }
        }
        public bool IsEditable
        {
            get
            {
                return (Status == CoachInterviewStatus.Booked && InterviewDate > DateTime.Now.AddHours(4));
            }
        }

        public CoachCalendar() { }

        public CoachCalendar(DataRow reader)
        {
            Id = reader["Id"].ToInt32();
            CoachId = reader["CoachId"].ToInt32();
            FullName = reader["FullName"].ToString();
            ProfilePhoto = reader["ProfilePhoto"].ToString();
            InterviewType = reader["InterviewType"].ToString();
            InterviewDate = reader["AvailableDate"].ToDateTime();
            Status = (CoachInterviewStatus)reader["Status"].ToInt32();
        }

    }
}
