using Emoda.PekiYaBen.Entity.Validation;
using OLCA.Infrastructure.CQS;
using System.ComponentModel.DataAnnotations;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class SavePaymentCommand : ICommand
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductId { get; set; }

        [Required]
        [StringLength(250)]
        public string TransactionId { get; set; }

        [Required]
        public string Signature { get; set; }

        [StringLength(250)]
        public string Token { get; set; }

        [StringLength(15)]
        public string PurchaseTime { get; set; }
    }
}
