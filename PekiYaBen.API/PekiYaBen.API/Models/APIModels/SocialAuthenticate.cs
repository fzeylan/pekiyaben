using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.APIModels
{
    public class SocialAuthenticate
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        public string ProfilePhoto { get; set; }

        [Required]
        public string SocialToken { get; set; }

        [Required]
        public SocialMedia SocialMedia { get; set; }

        [Required]
        public string IPAddress { get; set; }
    }
}
