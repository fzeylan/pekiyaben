using System;

namespace PekiYaBen.WebSite.Models.ViewModels
{
    public class ContentLite
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Nullable<int> CoachId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
        public string Image { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int Status { get; set; }
    }
}