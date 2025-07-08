using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Validation;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Models.Entities
{
    public class Certificate
    {
        public string Term { get; set; }
        public string Organization { get; set; }
        public string Name { get; set; }
    }
}
