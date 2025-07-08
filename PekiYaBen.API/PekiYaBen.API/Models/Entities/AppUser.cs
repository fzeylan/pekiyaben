using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using PekiYaBen.API.Commands;
using PekiYaBen.API.Enums;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Validation;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        //[Required]
        [StringLength(100)]
        public string FullName { get; set; }

        //[Required]
        [StringLength(100)]
        public string Email { get; set; }

        //[Required]
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

        public Gender? Gender { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        public OccupationStatus? OccupationStatus { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public CommunicationPermission CommunicationPermission { get; set; }

        public DateTime? CommunicationPermissionUpdate { get; set; }

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

        public IList<object> GenderList
        {
            get
            {
                return EnumerationExtensions.ToList<Gender>();
            }
        }

        public IList<object> OccupationList
        {
            get
            {
                return EnumerationExtensions.ToList<OccupationStatus>();
            }
        }


        public IList<object> CommunicationPermissionList
        {
            get
            {
                return EnumerationExtensions.ToList<CommunicationPermission>();
            }
        }

        public IList<object> StatusList
        {
            get
            {
                return EnumerationExtensions.ToList<Status>();
            }
        }

        public IList<object> TransactionStatusList
        {
            get
            {
                var list = (List<dynamic>)EnumerationExtensions.ToList<TransactionStatus>();
                List<object> statList = new List<object>();
                statList.Add(new { Key = list[0].Key, Value = list[0].Value, Color = "#e13f2a" });
                statList.Add(new { Key = list[1].Key, Value = list[1].Value, Color = "#19a5c9" });
                statList.Add(new { Key = list[2].Key, Value = list[2].Value, Color = "#c9ae19" });

                return statList;
            }
        }

        public string[] CityList
        {
            get
            {
                return new string[] { "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Aksaray", "Amasya", "Ankara", "Antalya", "Ardahan", "Artvin", "Aydın", "Balıkesir", "Bartın", "Batman", "Bayburt", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Düzce", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Iğdır", "Isparta", "İstanbul", "İzmir", "Kahramanmaraş", "Karabük", "Karaman", "Kars", "Kastamonu", "Kayseri", "Kırıkkale", "Kırklareli", "Kırşehir", "Kilis", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Mardin", "Mersin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Osmaniye", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Şanlıurfa", "Şırnak", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak", "Türkiye Dışı" };
            }
        }

        public AppUser() { }

        public AppUser(DataRow reader, bool includePassword = false)
        {
            Id = reader["Id"].ToInt32();
            FullName = reader["FullName"].ToString();
            Email = reader["Email"].ToString();
            if (reader["ProfilePhoto"] != DBNull.Value)
            {
                var image = (byte[])reader["ProfilePhoto"];
                ProfilePhoto = Convert.ToBase64String(image, 0, image.Length);
            }
            ActivationCode = reader["ActivationCode"].ToString();
            DateOfBirth = reader["DateOfBirth"].ToNullDateTime();
            
            if (!reader.IsNull("Gender"))
                Gender = (Gender)reader["Gender"];

            PhoneNumber = reader["PhoneNumber"].ToString();
            Occupation = reader["Occupation"].ToString();

            if (!reader.IsNull("OccupationStatus"))
                OccupationStatus = (OccupationStatus)reader["OccupationStatus"];

            City = reader["City"].ToString();
            CommunicationPermission = (CommunicationPermission)reader["CommunicationPermission"].ToInt32();
            CommunicationPermissionUpdate = reader["CommunicationPermissionUpdate"].ToDateTime();
            LastLoginDate = reader["LastLoginDate"].ToNullDateTime();
            LastLoginIp = reader["LastLoginIp"].ToString();
            SocialMedia = (SocialMedia)reader["SocialMedia"].ToInt32();
            SocialMediaToken = reader["SocialMediaToken"].ToString();
            Token = reader["Token"].ToString();
            FCMToken = reader["FCMToken"].ToString();
            Status = (Status)reader["Status"].ToInt32();
            if (includePassword)
            {
                Password = reader["Password"].ToString();
            }
        }
    }
}
