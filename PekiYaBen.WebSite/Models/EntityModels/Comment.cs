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
    
    public partial class Comment
    {
        public int Id { get; set; }
        public Nullable<int> CoachId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Comment1 { get; set; }
        public Nullable<int> Stars { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
