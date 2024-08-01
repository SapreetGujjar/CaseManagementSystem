// Sent to the client when a case is created.
using CaseManagementSystem.Helpers;
using CaseManagementSystem.Pages;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Reflection.Metadata;

namespace CaseManagementSystem.Emails.Templates
{
    public class NewAccountTemplate : BaseEMailTemplate, IEmailTemplate
    {
        private string subject = @"Account created";
        public string siteUrl = "[siteUrl]";
        public string userFirstName = "[First name]";
        public string PasswordResetRequestId = "[PasswordResetRequestId]";
        public string GeneratedPassword = "[GeneratedPassword]";
        public string emailAddress { get; } = "[emailAddress]";
        public string ccEmail { get; } = "[ccEmail]";
        public async Task Initialized(string siteUrl, string userFirstName, Guid passwordResetRequestId, string Password, string emailAddress, string ccEmail)
        {
            CryptoService cs = new CryptoService();
            body = body
                .Replace(siteUrl, siteUrl, StringComparison.InvariantCultureIgnoreCase)
                .Replace(userFirstName, userFirstName, StringComparison.InvariantCultureIgnoreCase)
                 .Replace(emailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase)
                 .Replace(ccEmail, ccEmail, StringComparison.InvariantCultureIgnoreCase)
                .Replace(PasswordResetRequestId, cs.EncryptStringEscaped(passwordResetRequestId.ToString()), StringComparison.InvariantCultureIgnoreCase)
                .Replace(GeneratedPassword, Password.ToString(), StringComparison.InvariantCultureIgnoreCase);
            body += "<br/><br/>" + this.footer;
            await AccountBody(siteUrl, userFirstName, passwordResetRequestId, Password, emailAddress, ccEmail);
        }
        private async Task<string> AccountBody(string siteUrl, string userFirstName, Guid passwordResetRequestId, string Password, string emailAddress, string ccEmail)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-e3985c46e1644458a294fb9669b2e309");
                var templateData = new
                {
                    siteUrl,
                    userFirstName,
                    passwordResetRequestId,
                    Password,
                    Subject
                };
                var response = await SendEmail(Subject, templateData, emailAddress, new List<string> { ccEmail });
                return response ? "Email sent successfully!" : throw new Exception("Failed to send email.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<bool> SendEmail(string subject, object templateData, string to, List<string> cc)
        {
            try
            {
                var templatedata = templateData;
                string apiKey = "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";
                var client = new SendGrid.SendGridClient(apiKey);
                var from = new EmailAddress("liam.geoghegan@esarisk.com", "Admin");
                var toList = new EmailAddress(to);
                var templateId = "d-e3985c46e1644458a294fb9669b2e309";
                var msg = MailHelper.CreateSingleTemplateEmail(from, toList,  templateId, templatedata);
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
        }
        private string body = "";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }

    }
}
