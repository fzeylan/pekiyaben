using OLCA.Infrastructure.CQS;
using System.ComponentModel.DataAnnotations;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class ValidateUserCommand : ICommand
    {
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
