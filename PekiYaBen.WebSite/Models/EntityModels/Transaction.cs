//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PekiYaBen.WebSite.Models.EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int ProductId { get; set; }
        public int MarketId { get; set; }
        public string TransactionId { get; set; }
        public string TransactionDetails { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal TransactionPrice { get; set; }
        public int Status { get; set; }
    }
}
