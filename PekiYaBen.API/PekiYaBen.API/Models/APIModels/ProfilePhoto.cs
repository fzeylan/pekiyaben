using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class ProfilePhoto
    {
        [Required]
        [FromForm(Name = "photo")]
        public IFormFile Photo { get; set; }
    }
}
