using Emoda.PekiYaBen.Entity.Validation;
using OLCA.Infrastructure.CQS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class UpdateUserCommand : ICommand
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NameSurname { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DateBeforeThanNow]
        public int BirthDate { get; set; }

        [StringLength(25)]
        public string City { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string Job { get; set; }

        [StringLength(100)]
        public string Cellphone { get; set; }

        [StringLength(100)]
        public string Sex { get; set; }

        public bool ComPermission { get; set; }

        [StringLength(10)]
        public string WorkStatus { get; set; }

    }
}
