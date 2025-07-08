using OLCA.Infrastructure.CQS;
using System.ComponentModel.DataAnnotations;

namespace Emoda.PekiYaBen.Business.Commands
{
    public class RenewPasswordCommand : ICommand
    {
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
