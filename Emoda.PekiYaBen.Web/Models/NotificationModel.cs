using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Models
{
    public class NotificationModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        //[Required]
        //public DateTime DueDate { get; set; }

        public string[] Users { get; set; }
    }
}