using Emoda.PekiYaBen.Entity.Enums;
using OLCA.Infrastructure.CQS;
using System;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class UpdateFAQCommand : ICommand
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public General.Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
       

    }
}
