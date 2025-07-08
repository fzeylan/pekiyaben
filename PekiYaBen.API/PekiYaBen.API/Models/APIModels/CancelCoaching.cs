using System;
using System.ComponentModel.DataAnnotations;

namespace PekiYaBen.API.Models.APIModels
{
    public class CancelCoaching
    {
        [Required]
        public int Id{ get; set; }
    }
}
