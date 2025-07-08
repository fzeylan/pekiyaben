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
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Commands
{
    public class AppUserCommand
    {
        private readonly AppSettings _settings;
        public AppUserCommand(AppSettings configuration)
        {
            _settings = configuration;
        }

        public CommandResponse Login(string Email, string Password, string IPAddress)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                string loginQuery = $"SELECT TOP 1 * FROM AppUser WHERE Email=@Email AND Password=@Password AND Status=@Status";

                var loginParameters = new List<SqlParameter> {
                new SqlParameter("Email", Email),
                new SqlParameter("Password", HashHelper.ConvertToMd5x(Password)),
                new SqlParameter("Status", Status.Active.GetValue())
            };

                var loginResult = da.GetDataReader(loginQuery, loginParameters);
                if (loginResult != null && loginResult.Rows.Count > 0) //set last login and last login IP
                {
                    var loginUser = new AppUser(loginResult.Rows[0]);

                    if (String.IsNullOrEmpty(loginUser.SocialMediaToken))
                    {

                        string lastLoginQuery = "Update AppUser SET LastLoginDate=@LastLoginDate, LastLoginIP=@LastLoginIp WHERE ID=@Id";

                        var lastLoginParameters = new List<SqlParameter> {
                        new SqlParameter("Id", loginUser.Id),
                        new SqlParameter("LastLoginDate", DateTime.Now),
                        new SqlParameter("LastLoginIP", IPAddress)
                    };

                        da.ExecuteNonQuery(lastLoginQuery, lastLoginParameters);

                        response = new CommandResponse { Data = loginUser, Succeed = true, };
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Title = "Kullanıcı Girişi", Message = "Kullanıcı için sosyal medya hesabı tanımlandığından e-posta ile giriş yapılamıyor.", Succeed = false };
                    }

                }
                else
                {
                    response = new CommandResponse { Data = null, Succeed = false, Title = "Kullanıcı Girişi", Message = "Kullanıcı girişi başarısız oldu. E-posta ve şifrenizi kontrol ederek tekrar deneyiniz. Şifrenizi unuttuysanız, hatırlatma ekranı ile ilerleyebilirsiniz." };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request Login Email:" + Email + ", Pass :" + Password + ", Error : " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "Kullanıcı girişi yapılırken hata oluştu !", Succeed = false };
            }

            return response;
        }

        public CommandResponse SocialLogin(string Email, string FullName, string ProfilePhoto, string SocialToken, SocialMedia Media, string IPAddress)
        {
            CommandResponse response = new CommandResponse();

            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                string socialEmail = "";
                if (Media == SocialMedia.Facebook)
                {
                    socialEmail = VerifyFacebookToken(SocialToken);
                }
                else if (Media == SocialMedia.Google)
                {
                    socialEmail = VerifyGoogleToken(SocialToken);
                }
                else if (Media == SocialMedia.Apple)
                {
                    socialEmail = Email;
                }

                socialEmail = socialEmail.Trim();

                if (string.IsNullOrEmpty(socialEmail))
                    return new CommandResponse { Data = null, Title = "Oturum Sona Erdi", Message = "Sosyal medya oturumu zaman aşımına uğradı. Yeniden giriş yapınız.", Succeed = false };

                if (socialEmail != Email.Trim())
                    return new CommandResponse { Data = null, Title = "Geçersiz Giriş", Message = "Geçersiz sosyal medya girişi denemesi. Yeniden giriş yapınız.", Succeed = false };

                var existingUser = da.ExecuteScalar("SELECT 1 FROM AppUser WHERE Email=@Email", new List<SqlParameter> { new SqlParameter("Email", Email), new SqlParameter("Status", Status.Active.GetValue()) }).ToInt32();

                if (existingUser > 0) //kullanıcı varsa
                {
                    string loginQuery = $"SELECT TOP 1 * FROM AppUser WHERE Email=@Email AND SocialMedia=@SocialMedia";

                    var loginParameters = new List<SqlParameter> {
                        new SqlParameter("Email", Email),
                        new SqlParameter("SocialMedia", Media.GetValue()),
                        new SqlParameter("Status", Status.Active.GetValue())
                    };

                    var loginResult = da.GetDataReader(loginQuery, loginParameters);
                    if (loginResult != null && loginResult.Rows.Count > 0) //sosyal medyadan mı eklenmiş?
                    {
                        var loginUser = new AppUser(loginResult.Rows[0]);
                        if (loginUser.Status == Status.Active) //aktif mi?
                        {
                            if (loginUser.SocialMediaToken.Equals(SocialToken)) //token doğru mu?
                            {
                                string lastLoginQuery = "Update AppUser SET LastLoginDate=@LastLoginDate, LastLoginIP=@LastLoginIp WHERE ID=@Id";

                                var lastLoginParameters = new List<SqlParameter> {
                                    new SqlParameter("Id", loginUser.Id),
                                    new SqlParameter("LastLoginDate", DateTime.Now),
                                    new SqlParameter("LastLoginIP", IPAddress)
                                };

                                da.ExecuteNonQuery(lastLoginQuery, lastLoginParameters);

                                response = new CommandResponse { Data = loginUser, Succeed = true };
                            }
                            else //token doğru değil
                            {
                                string lastLoginQuery = "Update AppUser SET SocialMediaToken=@SocialMediaToken, LastLoginDate=@LastLoginDate, LastLoginIP=@LastLoginIp WHERE ID=@Id";

                                var lastLoginParameters = new List<SqlParameter> {
                                    new SqlParameter("Id", loginUser.Id),
                                    new SqlParameter("SocialMediaToken", SocialToken),
                                    new SqlParameter("LastLoginDate", DateTime.Now),
                                    new SqlParameter("LastLoginIP", IPAddress)
                                };

                                da.ExecuteNonQuery(lastLoginQuery, lastLoginParameters);

                                response = new CommandResponse { Data = loginUser, Succeed = true };

                                //response = new CommandResponse { Data = null, Message = "Sosyal medya oturumu zaman aşımına uğradı. Yeniden giriş yapınız.", Succeed = false };
                            }
                        }
                        else //kullanıcı pasif
                        {
                            response = new CommandResponse { Data = null, Title = "Geçersiz Kullanıcı", Message = "Kullanıcı aktif durumda değil. Sistem yöneticileri ile irtibata geçiniz.", Succeed = false };
                        }
                    }
                    else //sosyal medyadan girmemiş?
                    {
                        response = new CommandResponse { Data = null, Title = "Geçersiz Kullanıcı", Message = "Kullanıcı için şifre tanımlandığından sosyal medya üzerinden giriş yapılamıyor. E-posta ve şifreniz ile giriş yapmayı deneyiniz.", Succeed = false };
                    }
                    //response = new CommandResponse { Data = null, Message = "Sosyal medya kullanıcı kaydı yapılamadı. Bu e-posta adresi ile kayıtlı bir kullanıcı bulunmaktadır.", Succeed = false };
                }
                else
                {
                    //add user
                    AppUser newUser = new AppUser()
                    {
                        FullName = FullName.Trim(),
                        Email = Email.Trim(),
                        Password = HashHelper.ConvertToMd5x(CreatePassword(8)),
                        SocialMediaToken = SocialToken,
                        SocialMedia = Media,
                        LastLoginDate = DateTime.Now,
                        LastLoginIp = IPAddress,
                        CommunicationPermission = Enums.General.CommunicationPermission.Denied,
                        CommunicationPermissionUpdate = DateTime.Now,
                        Status = Enums.General.Status.Active
                    };

                    byte[] image = new byte[0];
                    if (!string.IsNullOrEmpty(ProfilePhoto)) //profil fotosu var mı?
                    {
                        using (var client = new HttpClient())
                        {
                            image = client.GetByteArrayAsync(ProfilePhoto).Result;
                        }
                    }


                    string insertQuery = "INSERT INTO AppUser (FullName, Email, Password, CommunicationPermission, CommunicationPermissionUpdate, SocialMedia, SocialMediaToken, Status, LastLoginDate, LastLoginIp) VALUES(@FullName, @Email, @Password, @CommunicationPermission, @CommunicationPermissionUpdate, @SocialMedia, @SocialMediaToken, @Status, @LastLoginDate, @LastLoginIp); SELECT CAST(SCOPE_IDENTITY() as int)";

                    var insertParameters = new List<SqlParameter> {
                        new SqlParameter("FullName", newUser.FullName),
                        new SqlParameter("Email", newUser.Email),
                        new SqlParameter("Password", newUser.Password),
                        new SqlParameter("CommunicationPermission", newUser.CommunicationPermission),
                        new SqlParameter("CommunicationPermissionUpdate", newUser.CommunicationPermissionUpdate),
                        new SqlParameter("SocialMedia", newUser.SocialMedia),
                        new SqlParameter("SocialMediaToken", newUser.SocialMediaToken),
                        new SqlParameter("LastLoginDate", newUser.LastLoginDate),
                        new SqlParameter("LastLoginIp", newUser.LastLoginIp),
                        new SqlParameter("Status", newUser.Status)
                    };

                    if (image.Length > 0)
                    {
                        insertQuery = "INSERT INTO AppUser (FullName, Email, Password, CommunicationPermission, CommunicationPermissionUpdate, SocialMedia, SocialMediaToken, ProfilePhoto, Status, LastLoginDate, LastLoginIp) VALUES(@FullName, @Email, @Password, @CommunicationPermission, @CommunicationPermissionUpdate, @SocialMedia, @SocialMediaToken, @ProfilePhoto, @Status, @LastLoginDate, @LastLoginIp); SELECT CAST(SCOPE_IDENTITY() as int)";
                        insertParameters.Add(new SqlParameter("ProfilePhoto", image));
                    }

                    var insertResult = da.ExecuteScalar(insertQuery, insertParameters).ToInt32();
                    if (insertResult > 0) //kullanıcı eklenbildi mi?
                    {
                        var insertedUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Email=@Email", new List<SqlParameter> { new SqlParameter("Email", newUser.Email) });
                        if (insertedUser.Rows.Count > 0) //kullanıcı db'de var mı?
                        {
                            AppUser returnUser = new AppUser(insertedUser.Rows[0]);

                            //TODO: Social Login'de açalım.

                            //string lastLoginQuery = "Update AppUser SET LastLoginDate=@LastLoginDate, LastLoginIP=@LastLoginIp WHERE ID=@Id";

                            //var lastLoginParameters = new List<SqlParameter> {
                            //    new SqlParameter("Id", returnUser.Id),
                            //    new SqlParameter("LastLoginDate", DateTime.Now),
                            //    new SqlParameter("LastLoginIP", IPAddress)
                            //};
                            //da.ExecuteNonQuery(lastLoginQuery, lastLoginParameters);

                            response = new CommandResponse { Data = returnUser, Message = "Sosyal medya kullanıcısı başarılı bir şekilde eklendi", Succeed = true };
                        }
                        else //kullanıcı db'de yok
                        {
                            response = new CommandResponse { Data = null, Title = "Geçersiz Kullanıcı", Message = "Sosyal medya kullanıcısı eklenemedi", Succeed = false };
                        }
                    }
                    else //kullanıcı eklenemedi
                    {
                        response = new CommandResponse { Data = null, Title = "Geçersiz Kullanıcı", Message = "Sosyal medya kullanıcısı eklenemedi. Bilgilerde hata var!", Succeed = false };
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request SocialLogin Email:" + Email + ",Name :" + FullName + ", Token:" + SocialToken + ",Social:" + Media + ", Error : " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "Kullanıcı girişi yapılırken hata oluştu !", Succeed = false };
            }

            return response;
        }

        public CommandResponse VerifyToken(string SocialToken, SocialMedia Media)
        {
            CommandResponse response = new CommandResponse();

            string socialEmail = "";
            if (Media == SocialMedia.Facebook)
            {
                socialEmail = VerifyFacebookToken(SocialToken);
            }
            else if (Media == SocialMedia.Google)
            {
                socialEmail = VerifyGoogleToken(SocialToken);
            }

            if (!String.IsNullOrEmpty(socialEmail))
                response = new CommandResponse { Data = null, Message = "Oturum onaylandı", Succeed = true };
            else
                response = new CommandResponse { Data = null, Title = "Oturum Zaman Aşımı", Message = "Sosyal medya oturumu zaman aşımına uğradı. Yeniden giriş yapınız.", Succeed = false };

            return response;
        }

        private string VerifyFacebookToken(string SocialToken)
        {
            string response = "";
            try
            {
                WebRequest req = HttpWebRequest.Create("https://graph.facebook.com/me?fields=id,name,email&access_token=" + SocialToken);
                req.Method = "GET";

                string source;
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    source = reader.ReadToEnd();
                }

                dynamic resp = (dynamic)Newtonsoft.Json.Linq.JObject.Parse(source);
                if (!string.IsNullOrEmpty(resp.email.ToString()))
                {
                    response = resp.email.ToString();
                }

            }
            catch (Exception ex)
            {
                response = "";
            }

            return response;
        }

        private string VerifyGoogleToken(string SocialToken)
        {
            string response = "";
            try
            {
                WebRequest req = HttpWebRequest.Create("https://oauth2.googleapis.com/tokeninfo?id_token=" + SocialToken);
                req.Method = "GET";

                string source;
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    source = reader.ReadToEnd();
                }

                dynamic resp = (dynamic)Newtonsoft.Json.Linq.JObject.Parse(source);
                if (!string.IsNullOrEmpty(resp.email.ToString()))
                {
                    response = resp.email.ToString();
                }
            }
            catch (Exception ex)
            {
                response = "";
            }

            return response;
        }

        public CommandResponse VerifyPurchase(string token, string productId)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                const string PACKAGE_NAME = "com.emoda.pekiyaben";

                String serviceAccountEmail = "pekiyaben@pekiyaben-190110.iam.gserviceaccount.com";

                var certificate = new X509Certificate2(@"C:\pekiyaben-190110-7d9a4191b634.p12", "notasecret", X509KeyStorageFlags.Exportable);

                ServiceAccountCredential credential = new ServiceAccountCredential(
                     new ServiceAccountCredential.Initializer(serviceAccountEmail)
                     {
                         Scopes = new[] { "https://www.googleapis.com/auth/androidpublisher" }
                     }.FromCertificate(certificate));

                var service = new AndroidPublisherService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "pekiyaben",
                });
                var data = service.Purchases.Products.Get(PACKAGE_NAME, productId, token).Execute();

                //dynamic resp = (dynamic)Newtonsoft.Json.Linq.JObject.Parse(source);
                //if (!string.IsNullOrEmpty(resp.email.ToString()))
                //{
                //    response = resp.email.ToString();
                //}
                response.Succeed = true;
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request VerifyPurchase Token:" + token + ", productId :" + productId + ", Error : " + ex.ToString());

                response.Succeed = false;
            }

            return response;
        }

        public CommandResponse Register(AppUser user)
        {
            CommandResponse response = new CommandResponse();

            try
            {
                string errors = "";

                if (String.IsNullOrEmpty(user.FullName))
                    errors += "Ad Soyad alanı boş bırakılamaz\r\n";

                if (String.IsNullOrEmpty(user.Password))
                    errors += "Şifre alanı boş bırakılamaz\r\n";

                if (String.IsNullOrEmpty(user.Email))
                    errors += "E-posta alanı boş bırakılamaz\r\n";
                else
                {
                    if (!MailHelper.IsValid(user.Email))
                    {
                        errors = "Geçerli bir e-posta adresi girilmedir\r\n";
                    }
                }

                if (!String.IsNullOrEmpty(errors))
                    return new CommandResponse { Data = null, Title = "Hata Oluştu", Message = errors, Succeed = false };


                AppUser newUser = new AppUser()
                {
                    FullName = user.FullName.Trim(),
                    Email = user.Email.Trim(),
                    Password = HashHelper.ConvertToMd5x(user.Password),
                    CommunicationPermission = Enums.General.CommunicationPermission.Granted,
                    CommunicationPermissionUpdate = DateTime.Now,
                    Status = Enums.General.Status.Active
                };

                DataAccess da = new DataAccess(_settings.ConnectionString);
                var existingUser = da.ExecuteScalar("SELECT 1 FROM AppUser WHERE Email=@Email", new List<SqlParameter> { new SqlParameter("Email", newUser.Email) }).ToInt32();

                if (existingUser > 0)
                {
                    response = new CommandResponse { Data = null, Title = "Geçersiz Kullanıcı", Message = "Bu e-posta adresi ile kayıtlı bir kullanıcı bulunmaktadır.", Succeed = false };
                }
                else
                {
                    string insertQuery = "INSERT INTO AppUser (FullName, Email, Password, CommunicationPermission, CommunicationPermissionUpdate, Status) VALUES(@FullName, @Email, @Password, @CommunicationPermission, @CommunicationPermissionUpdate, @Status); SELECT CAST(SCOPE_IDENTITY() as int)";

                    var insertParameters = new List<SqlParameter> {
                        new SqlParameter("FullName", newUser.FullName),
                        new SqlParameter("Email", newUser.Email),
                        new SqlParameter("Password", newUser.Password),
                        new SqlParameter("CommunicationPermission", newUser.CommunicationPermission),
                        new SqlParameter("CommunicationPermissionUpdate", newUser.CommunicationPermissionUpdate),
                        new SqlParameter("Status", newUser.Status)
                    };

                    var insertResult = da.ExecuteScalar(insertQuery, insertParameters).ToInt32();
                    if (insertResult > 0)
                    {
                        var insertedUser = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Email=@Email", new List<SqlParameter> { new SqlParameter("Email", newUser.Email) });
                        if (insertedUser.Rows.Count > 0)
                        {
                            AppUser returnUser = new AppUser(insertedUser.Rows[0]);
                            response = new CommandResponse { Data = returnUser, Title = "Kullanıcı Eklendi", Message = "Kullanıcı başarılı bir şekilde eklendi", Succeed = true };
                        }
                        else
                        {
                            response = new CommandResponse { Data = null, Title = "Hata Oluştu", Message = "Kullanıcı eklenemedi", Succeed = false };
                        }
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Title = "Hata Oluştu", Message = "Kullanıcı eklenemedi : Kullanıcı bulunamadı!", Succeed = false };
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request Register user:" + JsonConvert.SerializeObject(user) + ", Error : " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "Kullanıcı eklerken hata oluştu !", Succeed = false };
            }
            return response;
        }

        public CommandResponse Update(UdpateUser user, string token)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                AppUser updateUser = new AppUser()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    Occupation = user.Occupation,
                    OccupationStatus = user.OccupationStatus,
                    City = user.City,
                    CommunicationPermission = user.CommunicationPermission,
                    CommunicationPermissionUpdate = DateTime.Now
                };

                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUpdate = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUpdate.Rows[0]);

                    string updateQuery = "UPDATE AppUser SET FullName=@FullName, DateOfBirth=@DateOfBirth, Gender=@Gender, PhoneNumber=@PhoneNumber, Occupation=@Occupation, OccupationStatus=@OccupationStatus, City=@City, CommunicationPermission=@CommunicationPermission, CommunicationPermissionUpdate=@CommunicationPermissionUpdate Where Id=@Id";

                    var updateParameters = new List<SqlParameter> {
                    new SqlParameter("Id", existingUser.Id),
                    new SqlParameter("FullName", updateUser.FullName),
                    new SqlParameter("DateOfBirth", updateUser.DateOfBirth),
                    new SqlParameter("Gender", updateUser.Gender),
                    new SqlParameter("PhoneNumber", updateUser.PhoneNumber),
                    new SqlParameter("Occupation", updateUser.Occupation),
                    new SqlParameter("OccupationStatus", updateUser.OccupationStatus),
                    new SqlParameter("City", updateUser.City),
                    new SqlParameter("CommunicationPermission", updateUser.CommunicationPermission),
                    new SqlParameter("CommunicationPermissionUpdate", existingUser.CommunicationPermission == updateUser.CommunicationPermission ? existingUser.CommunicationPermissionUpdate: updateUser.CommunicationPermissionUpdate)
                };

                    da.ExecuteNonQuery(updateQuery, updateParameters);

                    dtUpdate = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Id=@Id", new List<SqlParameter> { new SqlParameter("Id", existingUser.Id) });
                    if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                    {
                        AppUser newUser = new AppUser(dtUpdate.Rows[0]);
                        response = new CommandResponse { Title = "Kullanıcı Güncellendi", Message = "Kullanıcı başarılı bir şekilde güncellendi", Succeed = true, Data = newUser };
                    }
                    else
                    {
                        response = new CommandResponse { Title = "Kullanıcı Güncellendi", Message = "Kullanıcı başarılı bir şekilde güncellendi", Succeed = true };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "Kullanıcı güncellenirken hata oluştu : Kullanıcı bulunamadı (401)!", Succeed = false };

                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : Update token:" + token + ", Update:" + JsonConvert.SerializeObject(user) + ", Error : " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "Kullanıcı güncellenirken hata oluştu !", Succeed = false };
            }

            return response;
        }

        public CommandResponse UpdateProfilePhoto(string token, byte[] profilePhoto)
        {
            CommandResponse response = new CommandResponse();

            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUpdate = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUpdate.Rows[0]);

                    var image = profilePhoto.FromBytes().FixedSize(800, 800);

                    string updateQuery = "UPDATE AppUser SET ProfilePhoto=@ProfilePhoto Where Id=@Id";

                    var updateParameters = new List<SqlParameter> {
                        new SqlParameter("Id", existingUser.Id),
                        new SqlParameter("ProfilePhoto", image.ToBytes(ImageFormat.Jpeg))
                    };

                    da.ExecuteNonQuery(updateQuery, updateParameters);

                    response = new CommandResponse { Title = "Profil Resmi Güncellendi", Message = "Profil resmi başarılı bir şekilde güncellendi", Succeed = true };
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "Profil resmi güncellenirken hata oluştu : Kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : UpdateProfilePhoto token:" + token + ", Error : " + ex.ToString());

                response = new CommandResponse { Title = "Hata Oluştu", Message = "Profil resmi güncellenirken hata oluştu!", Succeed = false };
            }

            return response;
        }

        public void UpdateToken(int Id, string Token)
        {
            CommandResponse response = new CommandResponse();

            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                string lastLoginQuery = "Update AppUser SET Token=@Token WHERE Id=@Id";

                var lastLoginParameters = new List<SqlParameter> {
                    new SqlParameter("Id", Id),
                    new SqlParameter("Token", Token)
                };

                da.ExecuteNonQuery(lastLoginQuery, lastLoginParameters);
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : UpdateToken Id:" + Id + ", Token:" + Token + ", Error : " + ex.ToString());
            }
        }

        public CommandResponse ChangePassword(string token, ChangePassword pass)
        {
            CommandResponse response = new CommandResponse();

            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUpdate = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUpdate.Rows[0], true);

                    if (existingUser.Password == HashHelper.ConvertToMd5x(pass.OldPassword))
                    {
                        string updateQuery = "UPDATE AppUser SET Password=@Password Where Id=@Id";

                        var updateParameters = new List<SqlParameter> {
                        new SqlParameter("Id", existingUser.Id),
                        new SqlParameter("Password", HashHelper.ConvertToMd5x(pass.NewPassword))
                    };

                        da.ExecuteNonQuery(updateQuery, updateParameters);

                        response = new CommandResponse { Title = "Şifre Güncellendi", Message = "Şifreniz başarılı bir şekilde değiştirildi", Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Title = "Şifre Hatası", Message = "Eski şifreniz yanlış", Succeed = false };
                    }
                }
                else
                {
                    response = new CommandResponse { Title = "Hata Oluştu", Message = "Şifre değiştirilirken hata oluştu : Kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : ChangePassword Token:" + token + ", Pass:" + JsonConvert.SerializeObject(pass) + ", Error : " + ex.ToString());
                response = new CommandResponse { Data = null, Succeed = false, Title = "Hata oluştu", Message = "Token güncellenirken hata oluştu" };
            }

            return response;
        }

        public CommandResponse ResetPassword(string email)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUpdate = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Email=@Email", new List<SqlParameter> { new SqlParameter("Email", email) });
                if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUpdate.Rows[0], true);

                    if (String.IsNullOrEmpty(existingUser.SocialMediaToken))
                    {
                        string updateQuery = "UPDATE AppUser SET Password=@Password, Token=@Token Where Email=@Email AND SocialMedia IS NULL";

                        var newPassword = CreatePassword(8);
                        var updateParameters = new List<SqlParameter> {
                            new SqlParameter("Email", email),
                            new SqlParameter("Password", HashHelper.ConvertToMd5x(newPassword)),
                            new SqlParameter("Token", "")
                        };

                        da.ExecuteNonQuery(updateQuery, updateParameters);

                        var mailbody = MailHelper.PrepareMailBody("Şifremi Unuttum", $"<p>Talebiniz üzerine şifreniz sıfırlanmıştır. </p> <p>Yeni Şifreniz : {newPassword}</p>");

                        FCMService.SendNotification(new List<string> { existingUser.FCMToken }, "Şifremi Unuttum", $" Yeni Şifreniz : {newPassword}");


                        MailHelper.SendMail(email, "Şifremi unuttum", mailbody, _settings.FromEmail, _settings.Password, _settings.Host, _settings.Port);

                        response = new CommandResponse { Title = "Şifreniz Gönderildi", Message = "Şifreniz kayıtlı e-posta adresinize gönderildi ", Succeed = true };
                    }
                    else
                    {
                        response = new CommandResponse { Data = null, Succeed = false, Title = "Hata oluştu", Message = "Kullanıcı sosyal medya hesabından giriş yapmıştır, şifre gönderilemiyor" };
                    }
                }
                else
                {
                    response = new CommandResponse { Data = null, Succeed = false, Title = "Hata oluştu", Message = "Kullanıcı bulunamadı!" };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : ResetPassword " + email + " Error : " + ex.ToString());
                response = new CommandResponse { Data = null, Succeed = false, Title = "Hata oluştu", Message = "Şifre Değiştirilirken Hata oluştu" };
            }

            return response;
        }

        public CommandResponse FCMRegister(string token, string fcmToken)
        {
            CommandResponse response = new CommandResponse();

            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);

                var dtUpdate = da.GetDataReader("SELECT TOP 1 * FROM AppUser WHERE Token=@Token", new List<SqlParameter> { new SqlParameter("Token", token) });
                if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                {
                    AppUser existingUser = new AppUser(dtUpdate.Rows[0]);

                    string updateQuery = "UPDATE AppUser SET FCMToken=@FCMToken Where Id=@Id";

                    var updateParameters = new List<SqlParameter> {
                    new SqlParameter("Id", existingUser.Id),
                    new SqlParameter("FCMToken", fcmToken)
                };

                    da.ExecuteNonQuery(updateQuery, updateParameters);

                    response = new CommandResponse { Message = "FCM Token başarılı bir şekilde güncellendi", Succeed = true };
                }
                else
                {
                    response = new CommandResponse { Message = "FCM Token güncellenirken hata oluştu : Kullanıcı bulunamadı (401)!", Succeed = false };
                }
            }
            catch (Exception ex)
            {
                var logger = new SimpleLogger();
                logger.Info("Request : FCMRegister Token:" + token + ", FCM:" + fcmToken + ", Error : " + ex.ToString());
                response = new CommandResponse { Data = null, Succeed = false, Title = "Hata oluştu", Message = "Token güncellenirken hata oluştu" };
            }

            return response;
        }

        private string CreatePassword(int length)
        {
            Random random = new Random();
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
