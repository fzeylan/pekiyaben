using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class UserListItem
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }

        public Gender? Gender { get; set; }

        public string Occupation { get; set; }

        public string City { get; set; }

        public string Phonenumber { get; set; }

        public byte[] ProfilePhoto { get; set; }

        public CommunicationPermission CommunicationPermission { get; set; }

        public DateTime CommunicationPermissionUpdate { get; set; }

        public string Password { get; set; }

        public OccupationStatus? OccupationStatus { get; set; }

        public Status Status { get; set; }
        public SocialMedia? SocialMedia { get; set; }

        public int RecordCount { get; set; }
        public string FCMToken { get; set; }

        public DateTime? LastPurchaseDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
