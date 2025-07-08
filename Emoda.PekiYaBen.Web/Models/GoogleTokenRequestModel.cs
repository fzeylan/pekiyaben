using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emoda.PekiYaBen.Web.Models
{
    public class GoogleTokenRequestModel
    {
        public string grant_type { get { return "authorization_code"; } }

        public string code { get; set; }

        public static string client_id { get { return "874610646806-t39kktuah99rhb6gghbovdb88fogv1t6.apps.googleusercontent.com"; } }

        public static string client_secret { get { return "nlC6xlFhsSKmbH4L9VduWYr6"; } }

        public static string redirect_uri { get { return "https://mobileapp.pekiyaben.com/api/product/verify"; } }

    }

    public class GoogleTokenResponseModel
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires_in { get; set; }

        public string refresh_token { get; set; }
    }

    public class GoogleTokenRefreshRequestModel
    {
        public string grant_type { get { return "refresh_token"; } }

        public string client_id { get { return GoogleTokenRequestModel.client_id;  } }

        public string client_secret { get { return GoogleTokenRequestModel.client_secret; } }

        public string refresh_token { get; set; }
    }

    public class ProductPurchaseValidationModel
    {
        public string packageName { get { return "com.emoda.pekiyaben"; } }

        public string productId { get; set; }

        public string token { get; set; }
    }

    public class ProductPurchaseResponseModel
    {
        public string kind { get; set; }

        public long purchaseTimeMillis { get; set; }

        public int purchaseState { get; set; }

        public int consumptionState { get; set; }

        public string developerPayload { get; set; }

        public string orderId { get; set; }
    }

    public class ProductPurchaseResponseModelIOS
    {
        public int status { get; set; }

        public string receipt { get; set; }

        public string latest_receipt { get; set; }

        public string latest_receipt_info { get; set; }

        public string latest_expired_receipt_info { get; set; }

        public string pending_renewal_info { get; set; }

        public bool is_retryable { get; set; }
    }
}
