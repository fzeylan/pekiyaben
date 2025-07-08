using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class PaymentItem
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int MarketId { get; set; }
        public string TransactionId { get; set; }
        public string TransactionPrice { get; set; }
        public string TransactionDetails { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
