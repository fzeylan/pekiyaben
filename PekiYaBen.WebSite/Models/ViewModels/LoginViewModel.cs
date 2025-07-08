using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;

namespace PekiYaBen.WebSite.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta alanı boş olamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş olamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}