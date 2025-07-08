using System;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class ContentListItem
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public ContentType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public Status Status { get; set; }


        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int RecordCount { get; set; }

    }
}
