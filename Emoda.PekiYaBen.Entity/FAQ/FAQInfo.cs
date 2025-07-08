using Emoda.PekiYaBen.Entity.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Entity.FAQ
{
    public class FAQInfo
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
     
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [AllowHtml]
        public string Description { get; set; }


        public General.Status Status { get; set; }

        public DateTime CreatedDate { get; set; }
        
    }
}
