using PekiYaBen.WebSite.Extensions;
using PekiYaBen.WebSite.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static PekiYaBen.WebSite.Enums.General;

namespace PekiYaBen.WebSite.Models.ViewModels
{
    public class InvoiceUpdateViewModel
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }

        [Required]
        public InvoiceType InvoiceType { get; set; } = InvoiceType.Individual;

        public bool IsOtherCitizen { get; set; }

        [RequiredFor("InvoiceType", "0")]
        public string PersonalID { get; set; }

        [RequiredFor("InvoiceType", "1")]
        public string CompanyName { get; set; }

        [RequiredFor("InvoiceType", "1")]
        public string TaxNumber { get; set; }

        [RequiredFor("InvoiceType", "1")]
        public string TaxOffice { get; set; }

        [Required(ErrorMessage = "Adres boş olamaz")]
        [StringLength(100, ErrorMessage = "Adres 250 karakterden uzun olamaz")]
        public string InvoiceAddress { get; set; }

    }

    public class InvoiceUpdateInputModel
    {
        public InvoiceUpdateViewModel InvoiceInfo { get; set; }
    }
}