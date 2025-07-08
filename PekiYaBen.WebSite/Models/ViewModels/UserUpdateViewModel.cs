using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static PekiYaBen.WebSite.Enums.General;

namespace PekiYaBen.WebSite.Models.ViewModels
{
    public class UserUpdateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad boş olamaz")]
        [StringLength(100, ErrorMessage ="Ad Soyad 100 karakterden uzun olamaz")]
        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        [StringLength(15)]
        public string PhoneNumber{ get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(12, ErrorMessage = "Şifre 12 karakterden uzun olamaz")]
        [MinLength(6, ErrorMessage = "Şifre 6 karakterden kısa olamaz")]
        public string NewPassword { get; set; }

        [StringLength(12, ErrorMessage = "Şifre 12 karakterden uzun olamaz")]
        [MinLength(6, ErrorMessage = "Şifre 6 karakterden kısa olamaz")]
        public string NewPasswordAgain { get; set; }

        public string ProfilePhoto { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        public OccupationStatus? OccupationStatus { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public bool CommunicationPermission { get; set; }
    }
}