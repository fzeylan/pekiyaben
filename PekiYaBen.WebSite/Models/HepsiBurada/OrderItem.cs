using System;

namespace PekiYaBen.WebSite.Models.HepsiBurada
{

    public class Price
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }
    public class Discount
    {
        public Price TotalPrice { get; set; }
        public Price UnitPrice { get; set; }
    }
    public class CustomerAddress
    {
        public string AddressId { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
    }

    public class Invoice
    {
        public string TurkishIdentityNumber { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public CustomerAddress Address { get; set; }
    }


    public class CargoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LogoUrl { get; set; }
        public string TrackingUrl { get; set; }
    }

    public class Slot
    {
        public string Id { get; set; }
        public string Timeslot { get; set; }
    }

    public class OrderItem
    {
        public DateTime DueDate { get; set; }
        public DateTime LastStatusUpdateDate { get; set; }
        public string Id { get; set; }
        public string SKU { get; set; }
        public string ProductImageUrlFormat { get; set; }
        public int Quantity { get; set; }
        public int MerchantId { get; set; }
        public Price TotalPrice { get; set; }
        public Price UnitPrice { get; set; }
        public Discount HBDiscount { get; set; }
        public Price Vat { get; set; }
        public double VatRate { get; set; }
        public Price DiscountPriceToBeInvoicedHb { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public CustomerAddress ShippingAddress { get; set; }
        public Invoice Invoice { get; set; }
        public string SapNumber { get; set; }
        public decimal DispatchTime { get; set; }
        public Price Commission { get; set; }
        public int PaymentTermInDays { get; set; }
        public int CommissionType { get; set; }
        public CargoModel CargoCompanyModel { get; set; }
        public string CargoCompany { get; set; }
        public string CustomizedText01 { get; set; }
        public string CustomizedText02 { get; set; }
        public string CustomizedText03 { get; set; }
        public string CustomizedText04 { get; set; }
        public string CustomizedTextX { get; set; }
        public string CreditCardHolderName { get; set; }
        public bool IsCustomized { get; set; }
        public bool CanCreatePackage { get; set; }
        public bool IsCancellable { get; set; }
        public string DeliveryType { get; set; }
        public string deliveryOptionCode { get; set; }
        public Slot Slot { get; set; }
        public string PickUpTime { get; set; }
        public string MerchantSKU { get; set; }
        public Price PurchasePrice { get; set; }
        public string DeliveryNote { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

    }
}