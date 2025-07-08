using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Validation;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.Entities
{
    public class Interview
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int CoachId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string InterviewType { get; set; }
        public Status Status { get; set; }

        public Interview() { }

        public Interview(DataRow reader)
        {
            Id = reader["Id"].ToInt32();
            AppUserId = reader["AppUserId"].ToInt32();
            CoachId = reader["CoachId"].ToInt32();
            InterviewDate = reader["InterviewDate"].ToDateTime();
            InterviewType = reader["InterviewType"].ToString();
            Status = (Status)reader["Status"].ToInt32();
        }
    }
}