using OLCA.Infrastructure.CQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class SaveNotificationMessageCommand : ICommand
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string Content { get; set; }

        public DateTime DueDate { get; set; }

        public string[] UserInfoId { get; set; }
    }
}
