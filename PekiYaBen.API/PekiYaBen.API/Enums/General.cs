using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PekiYaBen.API.Enums
{
    public class General
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Gender
        {
            [Description("Erkek")] Male = 0,
            [Description("Kadın")] Female = 1
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OccupationStatus
        {
            [Description("Çalışmıyor")] Unemployed = 0,
            [Description("Çalışıyor")] Employed = 1
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Status
        {
            [Description("Pasif")] Passive = 0,
            [Description("Aktif")] Active = 1
        }

        public enum SocialMedia
        {
            Facebook = 0,
            Google = 1,
            Apple = 2
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum TransactionStatus
        {
            [Description("Pasif")] Passive = 0,
            [Description("Aktif")] Active = 1,
            [Description("Kullanıldı")] Used = 10

        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum CoachInterviewStatus
        {
            [Description("İptal Edildi")] Passive = 0,
            [Description("Randevu Alınabilir")] Active = 1,
            [Description("Randevu Alındı")] Booked = 2,
            [Description("Gerçekleşti")] Completed = 10
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ProductType
        {
            [Description("Product")] Product = 1,
            [Description("Audio")] Audio = 2,
            [Description("Meeting")] Meeting = 3
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum CommunicationPermission
        {
            [Description("Yok")] Denied = 0,
            [Description("Var")] Granted = 1
        }
    }
}
