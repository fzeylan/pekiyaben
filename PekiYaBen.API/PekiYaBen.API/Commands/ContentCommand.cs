using Emoda.PekiYaBen.Business.Helpers;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Models.APIModels;
using PekiYaBen.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Commands
{
    public class ContentCommand
    {
        private readonly AppSettings _settings;
        public ContentCommand(AppSettings configuration)
        {
            _settings = configuration;
        }

        public CommandResponse GetAnalyzeTitles(string token, int parentId)
        {
            CommandResponse response = new CommandResponse();
            List<Analyze> analyzeTitles = new List<Analyze>();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUser.Rows[0], true);

                    string analyzeQuery = $"SELECT * FROM Analyze WHERE ParentId=@ParentId AND Status=@Status ORDER BY SortOrder ASC";

                    var analyzeParameters = new List<SqlParameter> {
                        new SqlParameter("ParentId",parentId),
                        new SqlParameter("Status", Status.Active.GetValue())
                    };

                    var analyzeResult = da.GetDataReader(analyzeQuery, analyzeParameters);
                    if (analyzeResult != null && analyzeResult.Rows.Count > 0)
                    {
                        foreach (DataRow dr in analyzeResult.Rows)
                        {
                            analyzeTitles.Add(new Analyze(dr));
                        }
                        response = new CommandResponse { Data = analyzeTitles, Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetAnalyzeTitles " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse GetValues(string token)
        {
            CommandResponse response = new CommandResponse();
            List<Value> values = new List<Value>();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {

                    string query = $"SELECT * FROM Value ORDER BY SortOrder ASC";

                    var result = da.GetDataReader(query, new List<SqlParameter> { });
                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in result.Rows)
                        {
                            values.Add(new Value(dr));
                        }
                        response = new CommandResponse { Data = values, Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetValues " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse GetCoachingList(string token)
        {
            CommandResponse response = new CommandResponse();
            List<Product> values = new List<Product>();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUser.Rows[0], true);
                    var result = da.GetDataReader($"SELECT * FROM Product Where Type=3 AND Status=1 ORDER BY SortOrder ASC", new List<SqlParameter> { });
                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in result.Rows)
                        {
                            values.Add(new Product(dr));
                        }
                        response = new CommandResponse { Data = values, Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetCoachingList " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse GetAudioFiles(string token)
        {
            CommandResponse response = new CommandResponse();
            List<Product> audioFiles = new List<Product>();
            try
            {

                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUser.Rows[0], true);

                    string audioQuery = $"SELECT p.*,t.Id as tId FROM Product P LEFT JOIN [Transaction] t on t.ProductId=p.Id AND t.AppUserId=@UserId AND t.Status=1 WHERE Type=2 AND p.Status=@Status ORDER BY SortOrder ASC";

                    var audioParameters = new List<SqlParameter> { new SqlParameter("Status", Status.Active.GetValue()), new SqlParameter("UserId", existingUser.Id) };

                    var audioResult = da.GetDataReader(audioQuery, audioParameters);
                    if (audioResult != null && audioResult.Rows.Count > 0)
                    {
                        foreach (DataRow dr in audioResult.Rows)
                        {
                            var product = new Product(dr);
                            //User has bought the product
                            if (dr["tId"].ToInt32() > 0)
                                product.Available = true;

                            audioFiles.Add(product);
                        }
                        response = new CommandResponse { Data = audioFiles, Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetAudioFiles token:" + token + ", Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse SetPayment(string token, Payment payment)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUser.Rows[0], true);

                    if (existingUser.Id != payment.AppUserId)
                        return new CommandResponse { Message = "Kullanıcı işlem yapma yetkisine sahip değil!", Succeed = false };

                    var dtProduct = da.GetDataReader("SELECT TOP 1 * FROM Product WHERE Code=@Code", new List<SqlParameter> { new SqlParameter("Code", payment.ProductId) });
                    Product pr;
                    if (dtProduct != null && dtProduct.Rows.Count > 0)
                    {
                        pr = new Product(dtProduct.Rows[0]);
                    }
                    else
                        return new CommandResponse { Title = "Hata Oluştu", Message = "Satın almak istediğiniz ürün bulunamadı!", Succeed = false };

                    bool validPayment = true;
                    if (payment.MarketId == 1) //Play store
                    {
                        if (!string.IsNullOrEmpty(payment.TransactionDetails))
                        {
                            dynamic details = (dynamic)JsonConvert.DeserializeObject(payment.TransactionDetails);
                            string purchaseToken = details.purchaseToken.ToString();
                            if (!string.IsNullOrEmpty(purchaseToken))
                            {
                                validPayment = VerifyPlayStorePurchase(purchaseToken, pr.Code);
                            }
                            else
                                validPayment = false;
                        }
                        else
                            validPayment = false;
                    }

                    if (!validPayment)
                        return new CommandResponse { Title = "Hata Oluştu", Message = "İşleminizin yasal olmayan yollardan yapılmış olabileceğini düşünüyoruz. Devam edebilmek için ödemenize ait dekont veya google play store tarafından size gönderilen makbuzu iletisim@pekiyaben.com adresinden bizimle paylaşmanızı rica ediyoruz.", Succeed = false };

                    Transaction newPayment = new Transaction()
                    {
                        AppUserId = payment.AppUserId,
                        ProductId = pr.Id,
                        MarketId = payment.MarketId,
                        TransactionId = payment.TransactionId,
                        TransactionDetails = payment.TransactionDetails,
                        TransactionDate = DateTime.Now,
                        TransactionPrice = payment.TransactionId.Contains("PROMO") ? 0 : pr.Price,
                        Status = TransactionStatus.Active
                    };

                    string insertQuery = "Insert INTO [Transaction] (AppUserId, ProductId, MarketId, TransactionId, TransactionDetails, TransactionDate, TransactionPrice, Status) VALUES(@AppUserId, @ProductId, @MarketId, @TransactionId, @TransactionDetails, @TransactionDate, @TransactionPrice, @Status)";

                    var updateParameters = new List<SqlParameter> {
                        new SqlParameter("AppUserId", newPayment.AppUserId),
                        new SqlParameter("ProductId", newPayment.ProductId),
                        new SqlParameter("MarketId", newPayment.MarketId),
                        new SqlParameter("TransactionId", newPayment.TransactionId),
                        new SqlParameter("TransactionDetails", newPayment.TransactionDetails),
                        new SqlParameter("TransactionDate", newPayment.TransactionDate),
                        new SqlParameter("TransactionPrice", newPayment.TransactionPrice),
                        new SqlParameter("Status", newPayment.Status.GetValue())
                    };

                    da.ExecuteNonQuery(insertQuery, updateParameters);

                    response = new CommandResponse { Data = null, Succeed = true };
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {

                var logger = new SimpleLogger();
                logger.Info("Request : SetPayment token:" + token + ", payment:" + JsonConvert.SerializeObject(payment) + ", Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse GetUserProduct(string token, string productCode)
        {
            CommandResponse response = new CommandResponse();
            List<Transaction> products = new List<Transaction>();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUser.Rows[0], true);

                    if (!String.IsNullOrEmpty(productCode))
                    {
                        string productQuery = $"SELECT t.*, p.Title FROM [Transaction] t INNER JOIN Product p ON p.Id = t.ProductId AND p.Code=@ProductCode WHERE AppUserId=@UserId And t.Status=1";
                        //0:başarısız, 1:satın alındı, kullanılabilir, 100:kullanıldı

                        var productParameters = new List<SqlParameter> {
                            new SqlParameter("ProductCode", productCode),
                            new SqlParameter("UserId", existingUser.Id)
                        };

                        var result = da.GetDataReader(productQuery, productParameters);
                        if (result != null && result.Rows.Count > 0)
                        {
                            foreach (DataRow dr in result.Rows)
                            {
                                var pro = new Transaction(dr);
                                pro.ProductTitle = dr["Title"].ToString();
                                products.Add(pro);
                            }
                            response = new CommandResponse { Data = products, Succeed = true };
                        }
                        else
                        {
                            response = new CommandResponse { Data = null, Succeed = true };
                        }
                    }
                    else
                    {
                        string productQuery = $"SELECT t.*, p.Title FROM [Transaction] t INNER JOIN Product p ON p.Id = t.ProductId WHERE AppUserId=@UserId"; //0:başarısız, 1:satın alındı, kullanılabilir, 100:kullanıldı

                        var productParameters = new List<SqlParameter> { new SqlParameter("UserId", existingUser.Id) };

                        var result = da.GetDataReader(productQuery, productParameters);
                        if (result != null && result.Rows.Count > 0)
                        {
                            foreach (DataRow dr in result.Rows)
                            {
                                var pro = new Transaction(dr);
                                pro.ProductTitle = dr["Title"].ToString();
                                products.Add(pro);
                            }
                            response = new CommandResponse { Data = products, Succeed = true };
                        }
                        else
                        {
                            response = new CommandResponse { Data = null, Message = "Satın aldığınız bir ürün bulunamadı", Succeed = true };
                        }
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetUserProduct token:" + token + ", productCode:" + productCode + ", Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse SetInterview(string token, UserInterview interview)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);


                if (interview.InterviewDate >= DateTime.Now.AddHours(4))
                {
                    var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                    if (dtUser != null && dtUser.Rows.Count > 0)
                    {
                        AppUser user = new AppUser(dtUser.Rows[0], true);

                        if (user.Id != interview.AppUserId)
                            return new CommandResponse { Message = "Kullanıcı bilgileri eşleşmedi", Succeed = false };

                        Interview newInterview = new Interview()
                        {
                            AppUserId = interview.AppUserId,
                            InterviewDate = interview.InterviewDate,
                            InterviewType = interview.InterviewType,
                            Status = Status.Passive
                        };

                        var dtProduct = da.GetDataReader("SELECT TOP 1 * FROM Product WHERE Type=3 AND Title=@Title AND Status=1", new List<SqlParameter> { new SqlParameter("@Title", newInterview.InterviewType) });
                        if (dtProduct != null && dtProduct.Rows.Count > 0)
                        {
                            Product product = new Product(dtProduct.Rows[0]);

                            var dtTransaction = da.GetDataReader("SELECT TOP 1 * FROM [Transaction] WHERE AppUserId=@UserId AND ProductId=@ProductId AND Status=1",
                                    new List<SqlParameter> {
                                    new SqlParameter("@UserId", user.Id),
                                    new SqlParameter("@ProductId", product.Id)
                                    });

                            if (dtTransaction != null && dtTransaction.Rows.Count > 0)
                            {
                                Transaction transaction = new Transaction(dtTransaction.Rows[0]);

                                string insertQuery = "Insert INTO Interview (AppUserId, InterviewDate, InterviewType, Status) VALUES(@AppUserId, @InterviewDate, @InterviewType, @Status)";

                                var insertParameters = new List<SqlParameter> {
                                new SqlParameter("AppUserId", newInterview.AppUserId),
                                new SqlParameter("InterviewDate", newInterview.InterviewDate),
                                new SqlParameter("InterviewType", newInterview.InterviewType),
                                new SqlParameter("Status", newInterview.Status.GetValue())
                            };

                                da.ExecuteNonQuery(insertQuery, insertParameters);

                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                                sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");
                                sb.AppendFormat("<tr><th>Görüşme Tarihi</th><td>: {0}</td></tr>", interview.InterviewDate)
                                .AppendFormat("<tr><th>Koçluk Tipi</th><td>: {0}</td></tr>", interview.InterviewType)
                                .Append("</table>");

                                var mailbody = MailHelper.PrepareMailBody("Koçluk Görüşmesi Talebi", sb.ToString());

                                MailHelper.SendMail("fzeylan@yahoo.com", "Koçluk Görüşmesi Talebi", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                                MailHelper.SendMail("form@pekiyaben.com", "Koçluk Görüşmesi Talebi", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);

                                string updateQuery = "UPDATE [Transaction] SET Status=10 WHERE Id=@TransactionId";

                                da.ExecuteNonQuery(updateQuery, new List<SqlParameter> { new SqlParameter("TransactionId", transaction.Id) });

                                response = new CommandResponse { Data = null, Succeed = true, Title = "Formun gönderiliyor", Message = "Koçluk Görüşmesi talebiniz alınmıştır." };
                            }
                            else
                            {
                                response = new CommandResponse { Title = "Hata Oluştu", Message = "Randevu oluşturabilmek için koçluk ürünü satın almalısınız", Succeed = false };
                            }
                        }
                        else
                        {
                            response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz ürün bulunamadı", Succeed = false };
                        }
                    }
                    else
                    {
                        response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "Koçluk görüşmeleri için en erken 4 saat sonrasına randevu alabilirsiniz.", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : SetInterview token:" + token + ", interview:" + JsonConvert.SerializeObject(interview) + ", Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse WheelOfLife(string token, WheelOfLife wheel)
        {

            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser user = new AppUser(dtUser.Rows[0], true);

                    //kullanılabilir ürünü var mı?
                    string productQuery = $"SELECT TOP 1 t.* FROM [Transaction] t INNER JOIN Product p ON p.Id = t.ProductId AND p.Code=@ProductCode WHERE AppUserId=@UserId And t.Status=1";
                    //0:başarısız, 1:satın alındı, kullanılabilir, 100:kullanıldı

                    var productParameters = new List<SqlParameter> {
                            new SqlParameter("ProductCode", "com.emoda.pekiyaben.yasamcarki"),
                            new SqlParameter("UserId", user.Id)
                        };

                    var result = da.GetDataReader(productQuery, productParameters);
                    if (result != null && result.Rows.Count > 0)
                    {
                        Transaction transaction = new Transaction(result.Rows[0]);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                        sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");

                        int i = 1;
                        var errorFields = "";
                        foreach (var analyze in wheel.Results)
                        {
                            sb.AppendFormat("<tr><th>Alan {0}</th><td>: {1}</td></tr>", i, analyze.Title)
                            .AppendFormat("<tr><th>Şu anki Puanı</th><td>: {0}</td></tr>", analyze.CurrentScore)
                            .AppendFormat("<tr><th>Hedef Puanı</th><td>: {0}</td></tr>", analyze.ExpectedScore)
                            .Append("<tr><td colspan='2'><br></td></tr>");
                            i++;

                            if (analyze.CurrentScore > 0 && analyze.CurrentScore > analyze.ExpectedScore)
                            {
                                errorFields += analyze.Title + ", ";
                            }
                        }

                        if (!string.IsNullOrEmpty(errorFields))
                        {
                            return new CommandResponse { Title = "Hata Oluştu", Message = "Hedef puanınızın şu anki puanınızdan düşük olduğu alanlar belirledik. Lütfen puanlarınızı kontrol edip yeniden deneyiniz : " + errorFields, Succeed = false };
                        }

                        var selectedAnalyze = wheel.Results.FirstOrDefault(x => x.Selected);

                        sb.Append("<tr><th colspan=2><h3>Gerçeklik</h3><br/></th></tr>")
                        .AppendFormat("<tr><th>Seçilen Konu</th><td>: {0}</td></tr>", selectedAnalyze.Title)
                        .AppendFormat("<tr><th>Şu Anki Puan</th><td>: {0}</td></tr>", selectedAnalyze.CurrentScore)
                        .AppendFormat("<tr><th>Seçilen Alanın Anlamı</th><td>: {0}</td></tr>", wheel.SelectedComments)
                        .Append("<tr><td colspan='2'><br></td></tr>")
                        .Append("<tr><th colspan=2><h3>Gelecek</h3><br/></th></tr>")
                        .AppendFormat("<tr><th>Ulaşmak İstediği Puan</th><td>: {0}</td></tr>", selectedAnalyze.ExpectedScore)
                        .AppendFormat("<tr><th>Ulaşmak istediği tarih</th><td>: {0} {1}</td></tr>", wheel.ExpectedDateValue, wheel.ExpectedDateType)
                        .Append("<tr><td colspan='2'><br></td></tr>")
                        .AppendFormat("<tr><th>Puanın Anlamı</th><td>: {0}</td></tr>", wheel.ExpectedComments)
                        .AppendFormat("<tr><th>İhtiyaç Olunan Kaynaklar</th><td>: {0}</td></tr>", wheel.ExpectedResources)
                        .AppendFormat("<tr><th>Sahip Olunan Kaynaklar</th><td>: {0}</td></tr>", wheel.CurrentResources)
                        .AppendFormat("<tr><th>Eksik Olan Kaynaklar</th><td>: {0}</td></tr>", wheel.MissingResources)
                        .Append("</table>");


                        var mailbody = MailHelper.PrepareMailBody("Yaşam Çarkı", sb.ToString());

                        //MailHelper.SendMail("form@pekiyaben.com", "Yaşam Çarkı", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                        MailHelper.SendMail("fzeylan@yahoo.com", "Yaşam Çarkı", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                        MailHelper.SendMail("form@pekiyaben.com", "Yaşam Çarkı", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);

                        da.ExecuteNonQuery("UPDATE[Transaction] SET Status=10 WHERE Id = @Id AND AppUserId = @AppUserId", new List<SqlParameter> { new SqlParameter("AppUserId", user.Id), new SqlParameter("Id", transaction.Id) });

                        response = new CommandResponse { Data = null, Succeed = true, Title = "Formun gönderiliyor", Message = "1-3 iş günü içerisinde uluslararası akredite eğitimlere sahip deneyimli koçlarımızdan biri formunu koçluk bakış açısıyla değerlendirecek ve profil bilgilerinde yer alan e-posta adresine sonuçları gönderecek." };
                    }
                    else
                    {
                        response = new CommandResponse { Title = "Hata Oluştu", Message = "Aktif bir Yaşam Çarkı ürününüz bulunamadı! Satın alarak işleminize yeniden başlayabilirsiniz.", Succeed = false };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : SetInterview token:" + token + ", wheel:" + JsonConvert.SerializeObject(wheel) + ", Error:" + ex.ToString());

                response = new CommandResponse { Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse PersonalStrategy(string token, PersonalStrategy strategy)
        {

            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser user = new AppUser(dtUser.Rows[0], true);
                    //kullanılabilir ürünü var mı?
                    string productQuery = $"SELECT TOP 1 t.* FROM [Transaction] t INNER JOIN Product p ON p.Id = t.ProductId AND p.Code=@ProductCode WHERE AppUserId=@UserId And t.Status=1";
                    //0:başarısız, 1:satın alındı, kullanılabilir, 100:kullanıldı

                    var productParameters = new List<SqlParameter> {
                        new SqlParameter("ProductCode", "com.emoda.pekiyaben.kisiselstrateji"),
                        new SqlParameter("UserId", user.Id)
                    };

                    var result = da.GetDataReader(productQuery, productParameters);
                    if (result != null && result.Rows.Count > 0)
                    {
                        Transaction transaction = new Transaction(result.Rows[0]);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                        sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");

                        int i = 1;
                        foreach (var pesonalValue in strategy.Values.OrderBy(x => x.Order))
                        {
                            sb.AppendFormat("<tr><th>Alan {0}</th><td>: {1}</td></tr>", i, pesonalValue.Title)
                            .AppendFormat("<tr><th>Puanı</th><td>: {0}</td></tr>", pesonalValue.Order)
                            .AppendFormat("<tr><th>Anlamı</th><td>: {0}</td></tr>", pesonalValue.Comments);
                            sb.Append("<tr><td colspan='2'><br></td></tr>");
                            i++;
                        }

                        sb.AppendFormat("<tr><th>Vizyon</th><td>: {0}</td></tr>", strategy.Vision)
                        .AppendFormat("<tr><th>Misyon</th><td>: {0}<br/><br/></td></tr>", strategy.Mission)
                        .Append("</table>");

                        var mailbody = MailHelper.PrepareMailBody("Kişisel Strateji Formu", sb.ToString());
                        //MailHelper.SendMail("form@pekiyaben.com", "Kişisel Strateji Formu", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                        MailHelper.SendMail("fzeylan@yahoo.com", "Kişisel Strateji Formu", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                        MailHelper.SendMail("form@pekiyaben.com", "Kişisel Strateji Formu", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);

                        da.ExecuteNonQuery("UPDATE[Transaction] SET Status=10 WHERE Id = @Id AND AppUserId = @AppUserId", new List<SqlParameter> { new SqlParameter("AppUserId", user.Id), new SqlParameter("Id", transaction.Id) });

                        response = new CommandResponse { Data = null, Succeed = true, Title = "Formun gönderiliyor", Message = "1-3 iş günü içerisinde uluslararası akredite eğitimlere sahip deneyimli koçlarımızdan biri formunu koçluk bakış açısıyla değerlendirecek ve profil bilgilerinde yer alan e-posta adresine sonuçları gönderecek." };
                    }
                    else
                    {
                        response = new CommandResponse { Message = "Aktif bir Kişisel Strateji Belirleme ürününüz bulunamadı! Satın alarak işleminize yeniden başlayabilirsiniz.", Succeed = false };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : PersonalStrategy token:" + token + ", strategy:" + JsonConvert.SerializeObject(strategy) + ", Error:" + ex.ToString());

                response = new CommandResponse { Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }
            return response;
        }

        private bool VerifyPlayStorePurchase(string token, string productId)
        {
            bool response = false;
            var logger = new SimpleLogger();
            try
            {
                const string PACKAGE_NAME = "com.emoda.pekiyaben";

                String serviceAccountEmail = "pekiyaben@pekiyaben-190110.iam.gserviceaccount.com";

                string keyFile = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"P12KeyFile\pekiyaben-190110-7d9a4191b634.p12");
                logger.Info("Error : keyFile " + keyFile);

                var certificate = new X509Certificate2(keyFile, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

                ServiceAccountCredential credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail) { Scopes = new[] { "https://www.googleapis.com/auth/androidpublisher" } }.FromCertificate(certificate));

                var service = new AndroidPublisherService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "pekiyaben",
                });
                var data = service.Purchases.Products.Get(PACKAGE_NAME, productId, token).Execute();
                response = true;
            }
            catch (Exception ex)
            {

                logger.Error("Error : VerifyPlayStorePurchase " + ex.ToString());
                response = false;
            }
            return response;
        }

        public CommandResponse GetCoaches(string token, string productName)
        {
            CommandResponse response = new CommandResponse();
            List<Coach> values = new List<Coach>();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {

                    List<Product> coachingList = (List<Product>)GetCoachingList(token).Data;

                    var result = da.GetDataReader($"SELECT * FROM Coach Where Status=1 And CoachingInfo LIKE @Product", new List<SqlParameter> { new SqlParameter("Product", "%" + productName + "%") });
                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in result.Rows)
                        {
                            var coach = new Coach(dr);
                            string[] products = coach.CoachingInfo.Split(",");
                            foreach (string product in products)
                            {
                                var existingProduct = coachingList.FirstOrDefault(x => x.Title == product);
                                if (existingProduct != null)
                                {
                                    coach.CoachingList.Add(new CoachProduct { ProductId = existingProduct.Code, Title = existingProduct.Title, Price = existingProduct.Price });
                                }
                            }
                            values.Add(coach);
                        }

                        response = new CommandResponse { Data = values, Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = values, Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetCoaches productName:" + productName + ", Error:" + ex.ToString());

                response = new CommandResponse { Data = values, Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse GetCoachCalendar(string token, CoachDates request)
        {
            CommandResponse response = new CommandResponse();
            List<CoachDateResult> values = new List<CoachDateResult>();

            if (String.IsNullOrEmpty(request.InterviewType))
                return new CommandResponse { Title = "Uygulama güncelleme", Message = "Randevu ekranlarını sizin için iyileştirdik. Lütfen uygulamanızı son sürümüne güncelleyin.", Succeed = false };

            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {

                    List<Product> coachingList = (List<Product>)GetCoachingList(token).Data;
                    DateTime nextAvailableDate = DateTime.Now.AddHours(4);
                    DateTime startDate = request.StartDate < nextAvailableDate ? nextAvailableDate : request.StartDate;
                    DateTime endDate = startDate.AddDays(7);

                    //var interviewType = !request.InterviewType.Contains("Atölye")?"Genel"

                    var result = da.GetDataReader($"SELECT * FROM CoachCalendar cc Where cc.CoachId=@CoachId AND cc.AvailableDate>=@StartDate AND cc.AvailableDate<@EndDate " +
                        $"AND ParticipantLimit > (Select count(*) from CoachCalendarUser ccu Where CoachCalendarId=cc.Id And ccu.Status>=1) " +
                        $"AND InterviewType=" +
                        $"  CASE" +
                        $"      WHEN " +
                        $"          @InterviewType NOT LIKE '%Atölye%' AND (InterviewType='Genel' OR InterviewType=@InterviewType) AND InterviewType NOT LIKE '%Atölye%' " +
                        $"      THEN InterviewType " +
                        $"      WHEN @InterviewType LIKE '%Atölye%' AND InterviewType<>'Genel' THEN @InterviewType " +
                        $"  END " +
                        $"AND cc.Status=1",
                                                    new List<SqlParameter> {
                                                    new SqlParameter("CoachId", request.CoachId) ,
                                                    new SqlParameter("StartDate", startDate) ,
                                                    new SqlParameter("InterviewType", request.InterviewType) ,
                                                    new SqlParameter("EndDate", endDate)
                                                    }
                                                );

                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in result.Rows)
                        {
                            var coachDate = new CoachDateResult { StartDate = dr["AvailableDate"].ToDateTime(), CalendarId = dr["Id"].ToInt32() };
                            values.Add(coachDate);
                        }

                        response = new CommandResponse { Data = values, Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetCoachCalendar request:" + JsonConvert.SerializeObject(request) + ", Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse SetCoachInterview(string token, CoachInterview interview)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);


                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser user = new AppUser(dtUser.Rows[0], true);

                    var dtCoachInterview = da.GetDataReader("SELECT TOP 1 cc.*, c.FullName, ccu.* FROM CoachCalendar cc " +
                                                            "LEFT JOIN CoachCalendarUser ccu ON ccu.CoachCalendarId=cc.Id " +
                                                            "INNER JOIN Coach c ON c.Id=cc.CoachId " +
                                                            "WHERE cc.ParticipantLimit > (Select count(*) from CoachCalendarUser ccu Where CoachCalendarId=cc.Id And ccu.Status>=1) " +
                                                            "AND cc.Id=@Id AND cc.Status=1",
                                                            new List<SqlParameter> {
                                                                    new SqlParameter("Id", interview.CalendarId)
                                                            });
                    if (dtCoachInterview != null && dtCoachInterview.Rows.Count > 0)
                    {

                        string CoachName = dtCoachInterview.Rows[0]["FullName"].ToString();
                        string CoachCalendarId = dtCoachInterview.Rows[0]["Id"].ToString();
                        DateTime AvailableDate = dtCoachInterview.Rows[0]["AvailableDate"].ToDateTime();

                        if (AvailableDate >= DateTime.Now.AddHours(4))
                        {
                            var dtProduct = da.GetDataReader("SELECT TOP 1 * FROM Product WHERE Type=3 AND Title=@Title AND Status=1", new List<SqlParameter> { new SqlParameter("@Title", interview.InterviewType) });
                            if (dtProduct != null && dtProduct.Rows.Count > 0)
                            {
                                Product product = new Product(dtProduct.Rows[0]);

                                var dtTransaction = da.GetDataReader("SELECT TOP 1 * FROM [Transaction] WHERE AppUserId=@UserId AND ProductId=@ProductId AND Status=1",
                                        new List<SqlParameter> {
                                            new SqlParameter("@UserId", user.Id),
                                            new SqlParameter("@ProductId", product.Id)
                                        });

                                if (dtTransaction != null && dtTransaction.Rows.Count > 0)
                                {
                                    Transaction transaction = new Transaction(dtTransaction.Rows[0]);

                                    da.ExecuteNonQuery("Insert Into CoachCalendarUser (CoachCalendarId, AppUserId, TransactionId, InterviewType, Status) Values (@Id, @AppUserId, @TransactionId, @InterviewType, @Status)", new List<SqlParameter> {
                                                            new SqlParameter("Id", CoachCalendarId),
                                                            new SqlParameter("AppUserId", user.Id),
                                                            new SqlParameter("TransactionId", transaction.Id),
                                                            new SqlParameter("InterviewType", interview.InterviewType),
                                                            new SqlParameter("Status", CoachInterviewStatus.Booked)
                                                        });

                                    da.ExecuteNonQuery("UPDATE [Transaction] SET Status=10 WHERE Id=@TransactionId", new List<SqlParameter> { new SqlParameter("TransactionId", transaction.Id) });


                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                                    sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");
                                    sb.AppendFormat("<tr><th>Koç</th><td>: {0}</td></tr>", CoachName);
                                    sb.AppendFormat("<tr><th>Görüşme Tarihi</th><td>: {0}</td></tr>", AvailableDate)
                                    .AppendFormat("<tr><th>Koçluk Tipi</th><td>: {0}</td></tr>", interview.InterviewType)
                                    .Append("</table>");

                                    var mailbody = MailHelper.PrepareMailBody("Koçluk Görüşmesi Talebi", sb.ToString());

                                    MailHelper.SendMail("fzeylan@yahoo.com", "Koçluk Görüşmesi Talebi", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                                    MailHelper.SendMail("form@pekiyaben.com", "Koçluk Görüşmesi Talebi", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);

                                    response = new CommandResponse { Data = null, Succeed = true, Title = "Formun gönderiliyor", Message = "Koçluk Görüşmesi talebiniz alınmıştır." };
                                }
                                else
                                {
                                    response = new CommandResponse { Title = "Hata Oluştu", Message = "Randevu oluşturabilmek için koçluk ürünü satın almalısınız", Succeed = false };
                                }
                            }
                            else
                            {
                                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz ürün bulunamadı", Succeed = false };
                            }

                        }
                        else
                        {
                            response = new CommandResponse { Title = "Hata Oluştu", Message = "Koçluk görüşmeleri için en erken 4 saat sonrasına randevu alabilirsiniz.", Succeed = false };
                        }

                    }
                    else
                    {
                        response = new CommandResponse { Title = "Hata Oluştu", Message = "Koçumuz maalesef randevu almak istediğiniz ürün için belirtilen tarihte müsait değil. Başka bir tarih/saat için randevu almayı deneyebilirsiniz", Succeed = false };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }

            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : SetCoachInterview token:" + token + ", interview:" + JsonConvert.SerializeObject(interview) + ", Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse GetUserCoaching(string token)
        {
            CommandResponse response = new CommandResponse();
            List<CoachCalendar> values = new List<CoachCalendar>();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser user = new AppUser(dtUser.Rows[0], true);
                    var dtCoachInterview = da.GetDataReader("SELECT cc.Id, cc.AvailableDate, ccu.InterviewType, ccu.Status, c.Id CoachId, c.FullName, c.ProfilePhoto FROM CoachCalendar cc INNER JOIN CoachCalendarUser ccu ON ccu.CoachCalendarId =cc.Id INNER JOIN Coach c ON c.Id=cc.CoachId WHERE ccu.AppUserId=@AppUSerId", new List<SqlParameter> { new SqlParameter("AppUSerId", user.Id) });
                    if (dtCoachInterview != null && dtCoachInterview.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCoachInterview.Rows)
                        {
                            var coaching = new CoachCalendar(dr);
                            values.Add(coaching);
                        }
                        response = new CommandResponse { Data = values, Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = values, Message = "Herhangi bir koçluk randevusu bulunamadı", Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Data = values, Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : GetUserCoaching token:" + token + ", Error:" + ex.ToString());

                response = new CommandResponse { Data = values, Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse UpdateCoaching(string token, PostponeCoaching interview)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser user = new AppUser(dtUser.Rows[0], true);

                    var dtCoachInterview = da.GetDataReader("SELECT TOP 1 cc.Id, cc.AvailableDate, ccu.InterviewType, ccu.Id OldCalendarUserId, ccu.AppUserId,  ccu.TransactionId, ccu.Status, c.Id CoachId, c.FullName, c.ProfilePhoto FROM CoachCalendar cc INNER JOIN CoachCalendarUser ccu ON ccu.CoachCalendarId =cc.Id INNER JOIN Coach c ON c.Id=cc.CoachId WHERE ccu.AppUserId=@AppUSerId AND cc.Id=@Id AND ccu.Status=2",
                                                                new List<SqlParameter> {
                                                                        new SqlParameter("Id", interview.OldId),
                                                                        new SqlParameter("AppUserId", user.Id)
                                                                });

                    if (dtCoachInterview != null && dtCoachInterview.Rows.Count > 0)
                    {
                        int AppUserId = dtCoachInterview.Rows[0]["AppUserId"].ToInt32();
                        int TransactionId = dtCoachInterview.Rows[0]["TransactionId"].ToInt32();
                        string CoachName = dtCoachInterview.Rows[0]["FullName"].ToString();
                        string InterviewType = dtCoachInterview.Rows[0]["InterviewType"].ToString();
                        DateTime OldDate = dtCoachInterview.Rows[0]["AvailableDate"].ToDateTime();
                        int OldCalendarUserId = dtCoachInterview.Rows[0]["OldCalendarUserId"].ToInt32();

                        if (OldDate.AddHours(4) <= DateTime.Now)
                            return new CommandResponse { Message = "Randevunuza 4 saatten az bir zaman kaldığından güncellenemez.", Succeed = false };

                        if (user.Id != AppUserId)
                            return new CommandResponse { Message = "İşlem yapmak istediğiniz kullanıcı ile giriş yaptığınız kullanıcı bilgileri eşleşmedi", Succeed = false };

                        //yeni koçluk randevusu kontrolü

                        var dtNewCoachInterview = da.GetDataReader("SELECT TOP 1 cc.*, c.FullName, ccu.* FROM CoachCalendar cc " +
                                                                    "LEFT JOIN CoachCalendarUser ccu ON ccu.CoachCalendarId=cc.Id " +
                                                                    "INNER JOIN Coach c ON c.Id=cc.CoachId " +
                                                                    "WHERE cc.ParticipantLimit > (Select count(*) from CoachCalendarUser ccu Where CoachCalendarId=cc.Id And ccu.Status>=1) " +
                                                                    "AND cc.Id=@Id AND cc.Status=1",
                                                                new List<SqlParameter> {
                                                                    new SqlParameter("Id", interview.NewId)
                                                                });

                        if (dtNewCoachInterview != null && dtNewCoachInterview.Rows.Count > 0)
                        {
                            DateTime NewDate = dtNewCoachInterview.Rows[0]["AvailableDate"].ToDateTime();

                            if (NewDate >= DateTime.Now.AddHours(4))
                            {
                                var dtProduct = da.GetDataReader("SELECT TOP 1 * FROM Product WHERE Type=3 AND Title=@Title AND Status=1", new List<SqlParameter> { new SqlParameter("@Title", InterviewType) });
                                if (dtProduct != null && dtProduct.Rows.Count > 0)
                                {
                                    Product product = new Product(dtProduct.Rows[0]);

                                    var dtTransaction = da.GetDataReader("SELECT TOP 1 * FROM [Transaction] WHERE Id=@Id",
                                                                            new List<SqlParameter> {
                                                                                new SqlParameter("@Id", TransactionId)
                                                                            });

                                    if (dtTransaction != null && dtTransaction.Rows.Count > 0)
                                    {
                                        Transaction transaction = new Transaction(dtTransaction.Rows[0]);

                                        //Eskisini temizle
                                        da.ExecuteNonQuery("DELETE FROM CoachCalendarUser WHERE Id=@Id", new List<SqlParameter> {
                                                            new SqlParameter("Id", OldCalendarUserId)
                                                        });

                                        //yenisini ayarla

                                        da.ExecuteNonQuery("Insert Into CoachCalendarUser (CoachCalendarId, AppUserId, TransactionId, InterviewType, Status) Values (@Id, @AppUserId, @TransactionId, @InterviewType, @Status)", new List<SqlParameter> {
                                                            new SqlParameter("Id", interview.NewId),
                                                            new SqlParameter("AppUserId", user.Id),
                                                            new SqlParameter("TransactionId", transaction.Id),
                                                            new SqlParameter("InterviewType", InterviewType),
                                                            new SqlParameter("Status", CoachInterviewStatus.Booked)
                                                            });

                                        //da.ExecuteNonQuery("UPDATE CoachCalendar SET AppUserId=@AppUserId, InterviewType=@InterviewType, TransactionId=@TransactionId, Status=@Status WHERE Id=@Id", new List<SqlParameter> {
                                        //                    new SqlParameter("Id", interview.NewId),
                                        //                    new SqlParameter("AppUserId", AppUserId),
                                        //                    new SqlParameter("TransactionId", transaction.Id),
                                        //                    new SqlParameter("InterviewType", InterviewType),
                                        //                    new SqlParameter("Status", CoachInterviewStatus.Booked)
                                        //                });

                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                                        sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");
                                        sb.AppendFormat("<tr><th>Koç</th><td>: {0}</td></tr>", CoachName);
                                        sb.AppendFormat("<tr><th>Görüşme Tarihi</th><td>: {0}</td></tr>", NewDate)
                                        .AppendFormat("<tr><th>Eski Tarih</th><td>: {0}</td></tr>", OldDate)
                                        .AppendFormat("<tr><th>Koçluk Tipi</th><td>: {0}</td></tr>", InterviewType)
                                        .Append("</table>");

                                        var mailbody = MailHelper.PrepareMailBody("Koçluk Görüşmesi Güncelleme Talebi", sb.ToString());

                                        MailHelper.SendMail("fzeylan@yahoo.com", "Koçluk Görüşmesi Güncelleme", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                                        MailHelper.SendMail("form@pekiyaben.com", "Koçluk Görüşmesi Güncelleme", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);

                                        response = new CommandResponse { Data = null, Succeed = true, Title = "Formun gönderiliyor", Message = "Koçluk Görüşmesi güncelleme talebiniz alınmıştır." };
                                    }
                                    else
                                    {
                                        response = new CommandResponse { Title = "Hata Oluştu", Message = "Randevu oluşturabilmek için " + InterviewType + " satın almalısınız", Succeed = false };
                                    }
                                }
                                else
                                {
                                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz ürün bulunamadı", Succeed = false };
                                }
                            }
                            else
                            {
                                response = new CommandResponse { Title = "Hata Oluştu", Message = "Koçluk görüşmeleri için en erken 4 saat sonrasına randevu alabilirsiniz.", Succeed = false };
                            }
                        }
                        else
                        {
                            response = new CommandResponse { Title = "Hata Oluştu", Message = "Koçumuz randevu almak istediğiniz tarihte maalesef müsait değil. Başka bir tarih/saat için randevu almayı deneyebilirsiniz", Succeed = false };
                        }

                    }
                    else
                    {
                        response = new CommandResponse { Title = "Hata Oluştu", Message = "Güncellenmek istenen randevu bulunamadı", Succeed = false };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }

            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : UpdateCoaching token:" + token + ",interview:" + JsonConvert.SerializeObject(interview) + ",Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

        public CommandResponse CancelCoaching(string token, CancelCoaching interview)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    AppUser user = new AppUser(dtUser.Rows[0], true);

                    var dtCoachInterview = da.GetDataReader("SELECT TOP 1 cc.Id, cc.AvailableDate, ccu.InterviewType, ccu.Id OldCalendarUserId, ccu.AppUserId, ccu.TransactionId, ccu.Status, c.Id CoachId, c.FullName, c.ProfilePhoto FROM CoachCalendar cc INNER JOIN CoachCalendarUser ccu ON ccu.CoachCalendarId =cc.Id INNER JOIN Coach c ON c.Id=cc.CoachId WHERE ccu.AppUserId=@AppUSerId AND cc.Id=@Id AND ccu.Status=2",
                                                                new List<SqlParameter> {
                                                                        new SqlParameter("Id", interview.Id),
                                                                        new SqlParameter("AppUserId", user.Id)
                                                                });

                    if (dtCoachInterview != null && dtCoachInterview.Rows.Count > 0)
                    {
                        int AppUserId = dtCoachInterview.Rows[0]["AppUserId"].ToInt32();
                        int TransactionId = dtCoachInterview.Rows[0]["TransactionId"].ToInt32();
                        string CoachName = dtCoachInterview.Rows[0]["FullName"].ToString();
                        string InterviewType = dtCoachInterview.Rows[0]["InterviewType"].ToString();
                        DateTime InterviewDate = dtCoachInterview.Rows[0]["AvailableDate"].ToDateTime();
                        int OldCalendarUserId = dtCoachInterview.Rows[0]["OldCalendarUserId"].ToInt32();

                        if (InterviewDate.AddHours(24) <= DateTime.Now)
                            return new CommandResponse { Message = "Randevunuza 24 saatten az bir zaman kaldığından iptal edilemez.", Succeed = false };

                        if (user.Id != AppUserId)
                            return new CommandResponse { Message = "işlem yapmak istediğiniz kullanıcı ile giriş yaptığınız kullanıcı bilgileri eşleşmedi", Succeed = false };



                        var dtTransaction = da.GetDataReader("SELECT TOP 1 * FROM [Transaction] WHERE Id=@Id",
                                                                new List<SqlParameter> {
                                                                                new SqlParameter("@Id", TransactionId)
                                                                });

                        if (dtTransaction != null && dtTransaction.Rows.Count > 0)
                        {
                            Transaction transaction = new Transaction(dtTransaction.Rows[0]);

                            //Eskisini temizle
                            da.ExecuteNonQuery("DELETE FROM CoachCalendarUser WHERE Id=@Id", new List<SqlParameter> {
                                                            new SqlParameter("Id", OldCalendarUserId)
                                                        });

                            //servisi kullanılmamış duruma getir
                            da.ExecuteNonQuery("UPDATE [Transaction] SET Status=1 WHERE Id=@Id", new List<SqlParameter> {
                                                            new SqlParameter("Id", TransactionId)
                                                        });

                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                            sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");
                            sb.AppendFormat("<tr><th>Koç</th><td>: {0}</td></tr>", CoachName);
                            sb.AppendFormat("<tr><th>Görüşme Tarihi</th><td>: {0}</td></tr>", InterviewDate)
                            .AppendFormat("<tr><th>Koçluk Tipi</th><td>: {0}</td></tr>", InterviewType)
                            .Append("</table>");

                            var mailbody = MailHelper.PrepareMailBody("Koçluk Görüşmesi İptal Talebi", sb.ToString());

                            MailHelper.SendMail("fzeylan@yahoo.com", "Koçluk Görüşmesi İptal", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);
                            MailHelper.SendMail("form@pekiyaben.com", "Koçluk Görüşmesi İptal", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);

                            response = new CommandResponse { Data = null, Succeed = true, Title = "Formun gönderiliyor", Message = "Koçluk Görüşmesi iptal talebiniz alınmıştır." };
                        }
                        else
                        {
                            response = new CommandResponse { Title = "Hata Oluştu", Message = "Randevu iptali ile ilişkili satın alma bulunamadı", Succeed = false };
                        }
                    }
                    else
                    {
                        response = new CommandResponse { Title = "Hata Oluştu", Message = "İptal edilmek istenen randevu bulunamadı", Succeed = false };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem yapmak istediğiniz kullanıcı bulunamadı (401)!", Succeed = false };
                }

            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : CancelCoaching token:" + token + ",interview:" + JsonConvert.SerializeObject(interview) + ", Error:" + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "İşlem sırasında hata oluştu. Lütfen daha sonra tekrar deneyiniz", Succeed = false };
            }

            return response;
        }

    }
}