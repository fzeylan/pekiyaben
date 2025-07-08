using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using PekiYaBen.API.Commands;
using PekiYaBen.API.Enums;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Validation;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.Entities
{
    public class Coach
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

        public Coach() { }

        public Coach(DataRow reader)
        {
            Id = reader["Id"].ToInt32();
            FullName = reader["FullName"].ToString();
            Email = reader["Email"].ToString();
            ProfilePhoto = reader["ProfilePhoto"].ToString();
            DateOfBirth = reader["DateOfBirth"].ToNullDateTime();
            PhoneNumber = reader["PhoneNumber"].ToString();
            Description = reader["Description"].ToString();
            Title = reader["Title"].ToString();
            EducationInfo = reader["EducationInfo"].ToString();
            CertificateInfo = reader["CertificateInfo"].ToString();
            CoachingInfo = reader["CoachingInfo"].ToString();
            Status = (Status)reader["Status"].ToInt32();
        }
    }
}
