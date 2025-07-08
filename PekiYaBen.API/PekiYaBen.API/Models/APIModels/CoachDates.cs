using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class CoachDates
    {
        public int CoachId { get; set; }
        public string InterviewType { get; set; }
        public DateTime StartDate { get; set; }
    }
}