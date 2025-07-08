﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Emoda.PekiYaBen.Web.Models
{
    public class RegisterExternalTokenBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Token")]
        public string Token { get; set; }
        [Required]
        [Display(Name = "Provider")]
        public string Provider { get; set; }
    }
}