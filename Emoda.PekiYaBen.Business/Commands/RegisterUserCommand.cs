using Emoda.PekiYaBen.Entity.Validation;
using OLCA.Infrastructure.CQS;
using System.ComponentModel.DataAnnotations;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class RegisterUserCommand : ICommand
    {
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

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

    }
}
