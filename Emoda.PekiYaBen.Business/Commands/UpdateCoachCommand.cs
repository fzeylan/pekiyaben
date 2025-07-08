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
    public class UpdateCoachCommand : ICommand
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        //public string Password { get; set; }

        public string Title { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePhoto { get; set; }

        public string Description { get; set; }

        public string EducationInfo { get; set; }

        public string CoachingInfo { get; set; }

        public string CertificateInfo { get; set; }

        public Status Status { get; set; }
        public bool ShowMainPage { get; set; }

    }
}
