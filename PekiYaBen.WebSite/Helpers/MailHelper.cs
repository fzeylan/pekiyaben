using PekiYaBen.WebSite.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace PekiYaBen.WebSite.Helpers
{
    public static class MailHelper
    {
        public static void SendMail(string to, string subject, string body)
        {

            string from = ConfigurationSettings.AppSettings["FromEmail"].ToString();
            string password = ConfigurationSettings.AppSettings["Password"].ToString();
            string host = ConfigurationSettings.AppSettings["Host"].ToString();
            int port = ConfigurationSettings.AppSettings["Port"].ToInt32();

            MailMessage mail = new MailMessage();
            var mailFrom = new MailAddress(from, subject, Encoding.UTF8);
            var mailTo = new MailAddress(to);

            mail.Sender = new MailAddress(from);
            mail = new MailMessage(mailFrom, mailTo);
            mail.Attachments.Clear();
            mail.Bcc.Clear();
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(body));
            SmtpClient mailSender = new SmtpClient();
            mailSender.Host = host;
            mailSender.UseDefaultCredentials = true;
            mailSender.EnableSsl = false;

            mailSender.Credentials = new NetworkCredential(from, password);
            mailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailSender.Port = port;
            mailSender.Send(mail);
        }

        public static string PrepareMailBody(string title, string body)
        {
            string mailPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"Templates\email.html");
            string mailBody = "";
            if (File.Exists(mailPath))
            {
                mailBody = File.ReadAllText(mailPath, Encoding.GetEncoding("ISO-8859-9")).Replace("@Title", title).Replace("@Body", body);
            }
            else
            {
                mailBody = body;
            }

            return mailBody;
        }
    }
}