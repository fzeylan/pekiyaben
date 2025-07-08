using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.APIModels
{
    public class VerifyToken
    {
        [Required]
        public string SocialToken { get; set; }
        [Required]
        public SocialMedia SocialMedia { get; set; }
    }
}
