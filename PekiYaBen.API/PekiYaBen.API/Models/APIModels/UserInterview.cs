using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class UserInterview
    {

        [Required]
        public int AppUserId{ get; set; }
        [Required]
        public DateTime InterviewDate{ get; set; }
        [Required]
        public string InterviewType { get; set; }
    }
}
