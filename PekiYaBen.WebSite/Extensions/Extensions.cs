using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace PekiYaBen.WebSite.Extensions
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Verilen değeri decimal tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>Decimal</returns>
        public static Decimal ToDecimal(this object value)
        {
            return ToDecimal(value, 0);
        }

        /// <summary>
        /// Verilen değeri decimal tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <param name="defaultValue">Default değer</param>
        /// <returns>Decimal</returns>
        public static Decimal ToDecimal(this object value, decimal defaultValue)
        {
            decimal retVal = defaultValue;
            try
            {
                retVal = Convert.ToDecimal(value);
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Verilen değeri double tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>Double</returns>
        public static Double ToDouble(this object value)
        {
            return ToDouble(value, 0);
        }

        /// <summary>
        /// Verilen değeri decimal tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <param name="defaultValue">Default değer</param>
        /// <returns>Decimal</returns>
        public static Double ToDouble(this object value, double defaultValue)
        {
            Double retVal = defaultValue;
            try
            {
                Double.TryParse(value.ToString(), out retVal);
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Verilen değeri Int32 tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <param name="defaultValue">Default değer</param>
        /// <returns>Int32</returns>
        public static Int32 ToInt32(this object value, Int32 defaultValue)
        {
            Int32 retVal = defaultValue;
            try
            {
                retVal = Convert.ToInt32(value);
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Verilen değeri Int32 tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>Int32</returns>
        public static Int32 ToInt32(this object value)
        {
            return ToInt32(value, 0);
        }

        /// <summary>
        /// Verilen değeri Int64 tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <param name="defaultValue">Default değer</param>
        /// <returns>Int64</returns>
        public static Int64 ToInt64(this object value, Int64 defaultValue)
        {
            Int64 retVal = defaultValue;
            try
            {
                retVal = Convert.ToInt64(value);
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Verilen değeri Int64 tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>Int64</returns>
        public static Int64 ToInt64(this object value)
        {
            return ToInt64(value, 0);
        }

        /// <summary>
        /// Verilen değeri boolean tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <param name="defaultValue">Default değer</param>
        /// <returns>Boolean</returns>
        public static Boolean ToBoolean(this object value, Boolean defaultValue)
        {
            Boolean retVal = defaultValue;
            try
            {
                retVal = Convert.ToBoolean(value);
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Verilen değeri boolean tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <param name="defaultValue">Default değer</param>
        /// <returns>Boolean</returns>
        public static Boolean ToBoolean(this object value)
        {
            return ToBoolean(value, false);
        }

        public static Boolean ToIntBoolean(this object value)
        {
            return ToBoolean(value.ToInt32(), false);
        }

        /// <summary>
        /// En yakın komuşu sayıya yuvarlar
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int ToNearestNeighbor(this int i, int neighbor)
        {
            return ((int)Math.Ceiling(i / neighbor.ToDecimal())) * neighbor;
        }

        /// <summary>
        /// Verilen değeri boolean tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <param name="defaultValue">Default değer</param>
        /// <returns>Boolean</returns>
        public static String ToMoney(this decimal value, string extension = "TL")
        {
            return value.ToString("#0.00 " + extension);
        }
    }
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Gece yarısı itibarı ile bugüne ait yeni bir tarih değeri verir
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>DateTime</returns>
        public static DateTime ThisDay(this DateTime value)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        /// <summary>
        /// Önceki gün gece yarısı itibarı ile yeni bir tarih değeri verir
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>DateTime</returns>
        public static DateTime Yesterday(this DateTime value)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 0, 0, 0);
        }

        /// <summary>
        /// Tarih değeri boş ise null döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>DateTime</returns>
        public static DateTime? ToNullDateTime(this object value)
        {
            DateTime? retVal = null;
            try
            {
                DateTime date = Convert.ToDateTime(value);
                retVal = date == DateTime.MinValue ? null : (DateTime?)date;
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Verilen değeri decimal? tipte geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>Değer</returns>
        public static Decimal? ToNullDecimal(this object value)
        {
            Decimal? retVal = null;
            try
            {
                if (value == null)
                    retVal = null;
                else
                    retVal = Convert.ToDecimal(value);
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Verilen değeri datetime tipinde geri döndürür 
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this object value)
        {
            DateTime retVal = DateTime.MinValue;
            try
            {
                retVal = Convert.ToDateTime(value);
            }
            catch { }
            return retVal;
        }

        public static DateTime ToDateTime(this string value, string culture)
        {
            DateTime dt;
            if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.GetCultureInfo(culture), DateTimeStyles.AdjustToUniversal, out dt))
            {
            }
            return dt;
        }

        /// <summary>
        /// Verilen tarih değerini string tipinde geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>String</returns>
        public static String ToDayMonthYearHourMinute(this DateTime value, bool includeSeconds)
        {
            if (includeSeconds)
                return value.ToString("dd.MM.yyyy HH:mm:ss");
            else
                return value.ToString("dd.MM.yyyy HH:mm");
        }

        /// <summary>
        /// Verilen tarih değerini string tipinde geri döndürür
        /// </summary>
        /// <param name="value">Değer</param>
        /// <returns>String</returns>
        public static String ToDayMonthYear(this DateTime value)
        {
            return value.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Verilen tarih değerini, sql tarih formatına çevirir
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addHourMinuteSecond">Saat dakika ve saniye de eklensin mi?</param>
        /// <returns></returns>
        public static String ToSQLDate(this DateTime value, bool addHourMinuteSecond = false)
        {
            return value.ToString("yyyy-MM-dd" + (addHourMinuteSecond ? " HH:mm:ss" : ""));
        }

        /// <summary>
        /// Tarihin hafta sonu olup olmadığının kontrolü
        /// </summary>
        /// <param name="date">Tarih değeri</param>
        /// <returns>bool</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return new[] { DayOfWeek.Sunday, DayOfWeek.Saturday }.Contains(date.DayOfWeek);
        }

        public static void GetDates(DateTime dummy, string mode, out DateTime baslangic, out DateTime bitis)
        {
            baslangic = DateTime.Now;
            bitis = DateTime.Now;

            if (mode == "day")
            {
                baslangic = dummy;
                bitis = dummy;
            }
            else if (mode == "week")
            {
                baslangic = GetFirstDayOfWeek(dummy);
                bitis = GetLastDayOfWeek(dummy);
            }
            else
            {
                DateTime.TryParse("1." + dummy.Month.ToString() + "." + dummy.Year.ToString(), out baslangic);
                baslangic = GetFirstDayOfWeek(baslangic);
                DateTime.TryParse(DateTime.DaysInMonth(dummy.Year, dummy.Month) + "." + dummy.Month.ToString() + "." + dummy.Year.ToString(), out bitis);
            }
        }

        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.GetCultureInfo("tr-TR");
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }

        public static DateTime GetLastDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.GetCultureInfo("tr-TR");
            return GetLastDayOfWeek(dayInWeek, defaultCultureInfo);
        }

        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public static DateTime GetLastDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DateTime firstDayInWeek = GetFirstDayOfWeek(dayInWeek);
            return firstDayInWeek.AddDays(6);
        }

        public static DateTime ToUtcDateTime(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeToUtc(date);
        }

        public static DateTime ToUtcDateTime(this string date)
        {
            return ToUtcDateTime(DateTime.Parse(date));
        }
    }
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string input) => string.IsNullOrEmpty(input);
        public static bool IsNotNullOrEmpty(this string input) => !input.IsNullOrEmpty();
        public static bool IsNullOrWhiteSpace(this string input) => string.IsNullOrWhiteSpace(input);
        //TODO: Performance?
        public static bool IsEmail(this string email)
        {
            try
            {
                new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToTitleCase(this string s, CultureInfo UserCulture)
        {
            return UserCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static string ToLowerCase(this string s, CultureInfo UserCulture)
        {
            return UserCulture.TextInfo.ToLower(s);
        }
        public static string ToUpperCase(this string s, CultureInfo UserCulture)
        {
            return UserCulture.TextInfo.ToUpper(s);
        }

        public static string ToSEOFriendly(this string title, int maxLength)
        {
            title = title.Replace("İ", "I");
            title = title.ToLower();
            title = title.Replace("ğ", "g");
            title = title.Replace("ü", "u");
            title = title.Replace("ş", "s");
            title = title.Replace("ı", "i");
            title = title.Replace("ö", "o");
            title = title.Replace("ç", "c");

            var match = Regex.Match(title.ToLower(), "[\\w]+");
            StringBuilder result = new StringBuilder("");
            bool maxLengthHit = false;
            while (match.Success && !maxLengthHit)
            {
                if (result.Length + match.Value.Length <= maxLength)
                {
                    result.Append(match.Value + "-");
                }
                else
                {
                    maxLengthHit = true;
                    if (result.Length == 0) result.Append(match.Value.Substring(0, maxLength));
                }
                match = match.NextMatch();
            }
            if (result[result.Length - 1] == '-') result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        public static string EscapeSql(this string value)
        {
            if (value != null)
                value = value.Replace("'", "''").Trim();

            return String.IsNullOrWhiteSpace(value) ? null : value;
        }

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

    }
    public static class XmlExtensions
    {
        public static string AppendNode(string nodeName, string nodeValue)
        {
            return string.Format("<{0}>{1}</{0}>", nodeName, nodeValue);
        }
    }
    public static class EnumerationExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static int GetValue(this Enum value)
        {
            return value.ToInt32();
        }

        public static List<object> ToList<T>()
        {
            List<object> list = new List<object>();
            var enumValues = Enum.GetValues(typeof(T));
            foreach (int key in enumValues)
            {
                Enum value = (Enum)Enum.ToObject(typeof(T), key);
                list.Add(new { Key = key, Value = value.GetDescription() });
            }

            return list;
        }
    }
    public static class ImageExtensions
    {

        public static byte[] ImageFromFilePath(string source)
        {
            return Bitmap.FromFile(source).ToBytes(ImageFormat.Jpeg);
        }

        public static byte[] FromQuery(this string source)
        {
            string base64 = source.Substring(source.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            return Convert.FromBase64String(base64);
        }

        public static Image FromBytes(this byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        public static byte[] ToBytes(this System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static Image FixedSize(this Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
    public static class FormExtensions
    {
        public static IDictionary<string, object> ToDictionary(this NameValueCollection col)
        {
            var dict = new Dictionary<string, object>();

            foreach (string key in col.Keys)
            {
                dict.Add(key, col[key]);
            }
            return dict;
        }
    }
}
