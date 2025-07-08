using Emoda.PekiYaBen.Entity.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Entity.User
{
    public class AppUserInfo
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(32)]
        public string Password { get; set; }

        public string ProfilePhoto { get; set; }

        [StringLength(32)]
        public string ActivationCode { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthStr
        {
            get
            {
                return DateOfBirth != null ? DateOfBirth.ToDateTime().ToString("dd.MM.yyyy") : "";
            }
        }

        public Gender Gender { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        public OccupationStatus OccupationStatus { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public CommunicationPermission CommunicationPermission { get; set; }

        public DateTime CommunicationPermissionUpdate { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public string LastLoginDateStr
        {
            get
            {
                return LastLoginDate.ToDateTime().ToString("dd.MM.yyyy");
            }
        }

        public string LastLoginIp { get; set; }

        public Status Status { get; set; }

        public SocialMedia? SocialMedia { get; set; }

        public string SocialMediaToken { get; set; }

        public string Token { get; set; }
        public string FCMToken { get; set; }

        public DateTime RegisterDate { get; set; }


    }
}
