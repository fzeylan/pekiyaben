using Emoda.PekiYaBen.Entity.Helpers;
using PekiYaBen.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class CoachDateResult
    {
        public int CalendarId { get; set; }
        public DateTime StartDate { get; set; }
        public long StartDateTimeStamp
        {
            get
            {
                return StartDate.ToTimeStamp();
            }
        }
    }
}