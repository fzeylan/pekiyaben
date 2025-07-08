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
    public class Product
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
        public string File { get; set; }
        public decimal Price { get; set; }
        public int SortOrder { get; set; }
        public bool Available { get; set; }
        public ProductType Type { get; set; }
        public Status Status { get; set; }

        public Product() { }

        public Product(DataRow reader)
        {
            Id = reader["Id"].ToInt32();
            Code = reader["Code"].ToString();
            Type = (ProductType)reader["Type"].ToInt32();
            Title = reader["Title"].ToString();
            Description = reader["Description"].ToString();
            Price = reader["Price"].ToDecimal();
            Image = reader["Image"].ToString();
            File = reader["File"].ToString();
            Available = reader["Price"].ToDecimal() <= 0 ? true : false;
            SortOrder = reader["SortOrder"].ToInt32();
            Status = (Status)reader["Status"].ToInt32();
        }
    }
}