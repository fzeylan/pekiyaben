using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class InterviewListItem
    {
        public string CoachName { get; set; }
        public string UserName { get; set; }
        public string InterviewType { get; set; }
        public DateTime AvailableDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionPrice { get; set; }
        public CoachInterviewStatus Status { get; set; }
        public int RecordCount { get; set; }
        public InterviewPaymentType MarketId { get; set; }
    }
}
