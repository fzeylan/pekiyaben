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
    public class Analyze
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ParentId { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Icon { get; set; }
        public int SortOrder { get; set; }
        public Status Status { get; set; }

        public Analyze() { }

        public Analyze(DataRow reader)
        {
            Id = reader["Id"].ToInt32();
            ProductId = reader["ProductId"].ToInt32();
            ParentId = reader["ParentId"].ToInt32();
            Title = reader["Title"].ToString();
            Icon = reader["Icon"].ToString();
            SortOrder = reader["SortOrder"].ToInt32();
            Status = (Status)reader["Status"].ToInt32();
        }
    }
}
