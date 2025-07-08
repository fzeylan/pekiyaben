using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class CoachListItem
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Password { get; set; }

        public string Title { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePhoto { get; set; }

        public string Description { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string EducationInfo { get; set; }

        public string CoachingInfo { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string CertificateInfo { get; set; }


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

        public Status Status { get; set; }

        public DateTime RegisterDate { get; set; }

        public int RecordCount { get; set; }
    }

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
