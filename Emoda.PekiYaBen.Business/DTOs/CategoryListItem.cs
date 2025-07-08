using System;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class CategoryListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ContentType Type { get; set; }
        public Status Status { get; set; }

        public int RecordCount { get; set; }
    }
}
