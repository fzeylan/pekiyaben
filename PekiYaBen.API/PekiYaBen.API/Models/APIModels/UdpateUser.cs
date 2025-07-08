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
    public class UdpateUser
    {
        public int Id { get; set; }

        //[Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        public OccupationStatus? OccupationStatus { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public CommunicationPermission CommunicationPermission { get; set; }
    }
}
