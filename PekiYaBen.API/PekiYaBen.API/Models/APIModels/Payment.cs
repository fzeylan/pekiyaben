using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class Payment
    {
        [Required]
        public int AppUserId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int MarketId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string TransactionDetails { get; set; }
    }
}
