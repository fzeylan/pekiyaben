using Emoda.PekiYaBen.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emoda.PekiYaBen.Entity.Content
{
    public class ContentInfo
    {
        public int Id { get; set; }

        [Required] 
        public int CategoryId { get; set; }
        [Required]
        public int CoachId { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000)]
        public string Summary { get; set; }
        [Required]
        public string Detail { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public General.ContentType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public General.Status Status { get; set; }

        [StringLength(155)]
        public string MetaDescription { get; set; }

        [StringLength(100)]
        public string MetaKeywords { get; set; }

        public string ProductIds { get; set; }
    }
}
