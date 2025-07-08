using Emoda.PekiYaBen.Entity.Validation;
using OLCA.Infrastructure.CQS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class RemoveCoachCalendarCommand : ICommand
    {
        public int Id { get; set; }
    }
}
