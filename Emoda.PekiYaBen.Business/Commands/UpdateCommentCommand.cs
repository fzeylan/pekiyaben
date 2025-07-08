using OLCA.Infrastructure.CQS;
using System;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class UpdateCommentCommand : ICommand
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int ProductId { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public int Stars { get; set; }
        public DateTime CreatedDate { get; set; }
       

    }
}
