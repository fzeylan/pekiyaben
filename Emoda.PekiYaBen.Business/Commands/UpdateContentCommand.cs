using Emoda.PekiYaBen.Entity.Enums;
using OLCA.Infrastructure.CQS;
using System;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class UpdateContentCommand : ICommand
    {
        public int Id { get; set; }

        public int CoachId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public General.ContentType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public General.Status Status { get; set; }
        public string ProductIds { get; set; }

    }
}
