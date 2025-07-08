using Emoda.PekiYaBen.Entity.Validation;
using OLCA.Infrastructure.CQS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class UpdateCoachCalendarCommand : ICommand
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public string InterviewType { get; set; }
        public int ParticipantLimit { get; set; }
        public bool CoachIsFull { get; set; }

        public DateTime AvailableDate { get; set; }
        public CoachInterviewStatus Status { get; set; } =  CoachInterviewStatus.Active;

    }
}
