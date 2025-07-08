using System;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class CommentListItem
    {
        public int Id { get; set; }
        public string CoachFullName { get; set; }
        public string ProductTitle { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }        
        public int Stars { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public int RecordCount { get; set; }
    }
}
