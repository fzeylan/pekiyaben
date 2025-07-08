using System;
using System.ComponentModel.DataAnnotations;

namespace PekiYaBen.API.Models.APIModels
{
    public class PostponeCoaching
    {

        [Required]
        public int OldId{ get; set; }
        public int NewId { get; set; }
    }
}
