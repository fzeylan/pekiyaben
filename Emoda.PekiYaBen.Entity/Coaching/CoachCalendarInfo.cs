using System;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Entity.Coaching
{
    public class CoachCalendarInfo
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public DateTime AvailableDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ParticipantLimit { get; set; }
        public string InterviewType { get; set; }
        public CoachInterviewStatus Status { get; set; }
        public bool CoachIsFull { get; set; }
        
        public int TransactionId { get; set; }
        public int AppUserId { get; set; }
        public string FullName { get; set; }
    }
}
