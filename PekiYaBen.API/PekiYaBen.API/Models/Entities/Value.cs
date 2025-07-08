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
    public class Value
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
        public Status Status { get; set; }

        public Value() { }

        public Value(DataRow reader)
        {
            Id = reader["Id"].ToInt32();
            Title = reader["Title"].ToString();
            SortOrder = reader["SortOrder"].ToInt32();
            Status = (Status)reader["Status"].ToInt32();
        }
    }
}
