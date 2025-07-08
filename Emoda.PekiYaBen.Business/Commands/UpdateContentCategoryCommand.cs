using Emoda.PekiYaBen.Entity.Enums;
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
    public class UpdateContentCategoryCommand : ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public General.ContentType Type { get; set; }
        public General.Status Status { get; set; }

    }
}
