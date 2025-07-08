using System;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class FAQListItem
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public int RecordCount { get; set; }
    }
}
