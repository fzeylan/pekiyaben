using Emoda.PekiYaBen.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emoda.PekiYaBen.Entity.Content
{
    public class ContentCategoryInfo
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [Required]
        public General.ContentType Type { get; set; }
        public General.Status Status { get; set; }
    }
}
