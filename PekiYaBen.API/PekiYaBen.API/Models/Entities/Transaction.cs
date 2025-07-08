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
    public class Transaction
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int ProductId { get; set; }
        public int MarketId { get; set; }
        [StringLength(100)]
        public string TransactionId { get; set; }
        public string TransactionDetails { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateStr
        {
            get
            {
                return TransactionDate.ToString("dd.MM.yyyy HH:mm");
            }
        }
        public decimal TransactionPrice { get; set; }
        public TransactionStatus Status { get; set; }

        public string ProductTitle { get; set; }

        public Transaction() { }

        public Transaction(DataRow reader)
        {
            Id = reader["Id"].ToInt32();
            AppUserId = reader["AppUserId"].ToInt32();
            ProductId = reader["ProductId"].ToInt32();
            MarketId = reader["MarketId"].ToInt32();
            TransactionId = reader["TransactionId"].ToString();
            TransactionDetails = reader["TransactionDetails"].ToString();
            TransactionDate = reader["TransactionDate"].ToDateTime();
            TransactionPrice = reader["TransactionPrice"].ToDecimal();
            Status = (TransactionStatus)reader["Status"].ToInt32();
        }
    }
}