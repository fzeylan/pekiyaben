using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.APIModels
{
    public class VerifyPurchase
    {
        [Required]
        public string token { get; set; }
        [Required]
        public string productId { get; set; }
    }
}
