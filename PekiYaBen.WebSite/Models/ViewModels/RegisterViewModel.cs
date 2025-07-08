using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PekiYaBen.WebSite.Models.ViewModels
{
    public class RegisterUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad boş olamaz")]
        [StringLength(100, ErrorMessage = "Ad Soyad 100 karakterden uzun olamaz")]
        [DisplayName("Ad Soyad")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "E-posta boş olamaz")]
        [EmailAddress]
        [DisplayName("E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş olamaz")]
        [StringLength(12, ErrorMessage = "Şifre 12 karakterden uzun olamaz")]
        [MinLength(6, ErrorMessage = "Şifre 6 karakterden kısa olamaz")]
        [DisplayName("Şifre")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Şifre Tekrar boş olamaz")]
        [StringLength(12, ErrorMessage = "Şifre 12 karakterden uzun olamaz")]
        [MinLength(6, ErrorMessage = "Şifre 6 karakterden kısa olamaz")]
        [DisplayName("Şifre Tekrar")]
        public string NewPasswordAgain { get; set; }

        [Required(ErrorMessage = "Üyelik sözleşmesi onaylanmadan üye kaydı yapılamaz")]
        public bool ConfirmAgreement { get; set; }

    }
}