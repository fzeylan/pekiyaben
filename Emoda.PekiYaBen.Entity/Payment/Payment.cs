using System;

namespace Emoda.PekiYaBen.Entity.Payment
{
    public class Payment
    {
        public int UserId { get; set; }

        public string ProductId { get; set; }

        public string TransactionId { get; set; }

        public string Signature { get; set; }

        public string Token { get; set; }

        public string PurchaseTime { get; set; }

        public DateTime LogDate { get; set; }

    }
}
