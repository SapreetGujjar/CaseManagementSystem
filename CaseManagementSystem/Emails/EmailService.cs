using CaseManagementSystem.Emails.Templates;
using CaseManagementSystem.Pages;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using Hangfire;
using SendGrid.Helpers.Mail;

namespace CaseManagementSystem.Emails
{
    public class EmailService:IEmailService
    {
        private string _adminEmail, _from, _smtpServer, _userName, _password, _ccEmail;
        private int _port;
        public EmailService(string adminEmail, string from, string smtpServer, int port, string userName, string password , string ccEmail)
        { 
            //_adminEmail = adminEmail;
            //_from = from;
            //_smtpServer = smtpServer;
            //_userName = userName;
            //_password = password;
            //_port = port;
            //_ccEmail = ccEmail;
        }

        public async Task SendEMailNotificationToAdminAsync(IEmailTemplate notification)
        {
            await this.SendEmailNotificationAsync(notification, new List<string>() { _adminEmail }, null);
        }

        public async Task SendEmailNotificationAsync(IEmailTemplate notification, string to, string cc)
        {
            await this.SendEmailNotificationAsync(notification, new List<string>() { to }, new List<string>() { cc });
        }
        public async Task SendEmailNotificationAsync(IEmailTemplate notification, List<string> to, List<string> cc)
        {
            SendEmail(notification.Subject, notification.Body, to, cc);
        }

        //private void SendEmail(string subject, string body, List<string> to, List<string> cc)
        //{
        //    // to = new List<string>() { "enzopretti@hotmail.com" };
        //    // If email server is not setup, quit without erroring
        //    if (string.IsNullOrEmpty(_smtpServer))
        //        return;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    using (MailMessage mailMessage = new MailMessage())
        //    {
        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.Subject = subject;
        //        mailMessage.SubjectEncoding = Encoding.UTF8;
        //        mailMessage.BodyEncoding = Encoding.UTF8;
        //        mailMessage.Body = body;

        //        mailMessage.From = new MailAddress(_from);
        //        foreach(var item in to) {
        //            mailMessage.To.Add(new MailAddress(item));
        //        }
        //        // Adding CC recipients
        //        if (cc != null && cc.Count > 0&& cc[0] !=null)
        //        {
        //            foreach (var item in cc)
        //            {
        //                mailMessage.CC.Add(new MailAddress(item));
        //            }
        //        }
        //        using (SmtpClient smtpClient2 = new SmtpClient(_smtpServer))
        //        {
        //            smtpClient2.Port = _port;
        //            if (_userName.Trim() != string.Empty)
        //            {
        //                smtpClient2.UseDefaultCredentials = false;
        //                NetworkCredential smtpUserInfo = new NetworkCredential(_userName, _password);
        //                smtpClient2.Credentials = smtpUserInfo;
        //                smtpClient2.EnableSsl = true;
        //            }
        //            smtpClient2.Send(mailMessage);
        //        }
        //    }
        //}
        private async Task<bool> SendEmail(string subject, string body, List<string> to, List<string> cc)
        {
            try
            {
                string apiKey = "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";
                var client = new SendGrid.SendGridClient(apiKey);
                var from = new EmailAddress("liam.geoghegan@esarisk.com", "Admin");
                var toList = to.Select(email => new EmailAddress(email)).ToList();
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toList, subject, "", body);
                if (cc != null && cc.Any() && cc[0] != null)
                {
                    foreach (var ccEmail in cc)
                    {
                        msg.AddCc(ccEmail);
                    }
                }
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return true; // Email was successfully sent
                }
                else
                {
                    Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            //private void SendEmail(string subject, string body, List<string> to, List<string> cc)
            //{
            // to = new List<string>() { "enzopretti@hotmail.com" };
            // If email server is not setup, quit without erroring
            //if (string.IsNullOrEmpty(_smtpServer))
            //    return;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //using (MailMessage mailMessage = new MailMessage())
            //{
            //    mailMessage.IsBodyHtml = true;
            //    mailMessage.Subject = subject;
            //    mailMessage.SubjectEncoding = Encoding.UTF8;
            //    mailMessage.BodyEncoding = Encoding.UTF8;
            //    mailMessage.Body = body;

            //    mailMessage.From = new MailAddress(_from);
            //    foreach(var item in to) {
            //        mailMessage.To.Add(new MailAddress(item));
            //    }

            //    // Adding CC recipients
            //    if (cc != null && cc.Count > 0&& cc[0] !=null)
            //    {
            //        foreach (var item in cc)
            //        {
            //            mailMessage.CC.Add(new MailAddress(item));
            //        }
            //    }

            //    using (SmtpClient smtpClient2 = new SmtpClient(_smtpServer))
            //    {
            //        smtpClient2.Port = _port;
            //        if (_userName.Trim() != string.Empty)
            //        {
            //            smtpClient2.UseDefaultCredentials = false;
            //            NetworkCredential smtpUserInfo = new NetworkCredential(_userName, _password);
            //            smtpClient2.Credentials = smtpUserInfo;
            //            smtpClient2.EnableSsl = true;
            //        }

            //        smtpClient2.Send(mailMessage);
            //    }
            //}
        }
    }
}
