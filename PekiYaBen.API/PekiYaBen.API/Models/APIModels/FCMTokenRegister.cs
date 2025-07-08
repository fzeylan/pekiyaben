using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class FCMTokenRegister
    {

        [Required]
        public string FCMToken { get; set; }
    }
}
