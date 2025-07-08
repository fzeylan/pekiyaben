using Emoda.PekiYaBen.Entity.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Entity.Coaching
{
    public class ProductInfo
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Code { get; set; }
        [StringLength(150)]
        [Required]
        public string Title { get; set; }
        [Required]
        public ProductType Type { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
        [StringLength(250)]
        public string ContentDescription { get; set; }
        [StringLength(250)]
        public string ContentImage { get; set; }
        public string File { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int SortOrder { get; set; }
        public bool Available { get; set; }
        
        public Status Status { get; set; }
        public Boolean ShowMainPage { get; set; }
    }
}
