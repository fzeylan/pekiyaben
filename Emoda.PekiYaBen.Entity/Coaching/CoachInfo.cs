using Emoda.PekiYaBen.Entity.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Entity.Coaching
{
    public class CoachInfo
    {
        public int Id { get; set; }

        //[Required]
        [StringLength(100)]
        public string FullName { get; set; }

        //[Required]
        [StringLength(100)]
        public string Email { get; set; }

        //[Required]
        [StringLength(100)]
        [Newtonsoft.Json.JsonIgnore]
        public string Password { get; set; }

        public string ProfilePhoto { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthStr
        {
            get
            {
                return DateOfBirth != null ? DateOfBirth.ToDateTime().ToString("dd.MM.yyyy") : "";
            }
        }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string EducationInfo { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string CoachingInfo { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string CertificateInfo { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public Status Status { get; set; }

        public bool ShowMainPage { get; set; }
        public List<CoachProduct> CoachingList { get; set; } = new List<CoachProduct>();

        public List<Education> EducationList
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Education>>(EducationInfo);
            }
        }

        public List<Certificate> CertificateList
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Certificate>>(CertificateInfo);
            }
        }

        public CoachInfo() { }

        public class Education
        {
            public string Term { get; set; }
            public string Organization { get; set; }
            public string Name { get; set; }
        }

        public class Certificate
        {
            public string Term { get; set; }
            public string Organization { get; set; }
            public string Name { get; set; }
        }

        public class CoachProduct
        {
            public string ProductId { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
        }
    }
}
