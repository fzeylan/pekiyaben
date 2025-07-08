using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Emoda.PekiYaBen.Entity.Comment
{
    public class CommentInfo
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int ProductId { get; set; }
     
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        [StringLength(250)]
        [Required]
        public string Comment { get; set; }
   
        public int Stars { get; set; }

        public DateTime CreatedDate { get; set; }
        
    }
}
