using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PekiYaBen.WebSite.Models.ViewModels
{
    public class RemindPasswordViewModel
    {
        [Required(ErrorMessage = "E-posta boş olamaz")]
        [EmailAddress]
        [DisplayName("E-posta")]
        public string Email { get; set; }
    }
}