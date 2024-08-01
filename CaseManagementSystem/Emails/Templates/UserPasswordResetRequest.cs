// Sent to the client when a case is created.
using CaseManagementSystem.Helpers;
using CaseManagementSystem.Pages;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Reflection.Metadata;

namespace CaseManagementSystem.Emails.Templates
{
    public class UserPasswordResetRequest : BaseEMailTemplate, IEmailTemplate
    {
        public string siteUrl = "[siteUrl]";
        public string userFirstName = "[First name]";
        public string PasswordResetRequestId = "[PasswordResetRequestId]";
        private string subject = @"Password reset requested";
        public async Task Initialize(string siteUrl, string userFirstName, Guid passwordResetRequestId,string emailAddress)
        {
            CryptoService cs = new CryptoService();
            string encryptedRequestId = cs.EncryptStringEscaped(passwordResetRequestId.ToString());
            body = Body.Replace(siteUrl, siteUrl, StringComparison.InvariantCultureIgnoreCase)
                .Replace(userFirstName, userFirstName, StringComparison.InvariantCultureIgnoreCase)
                .Replace(PasswordResetRequestId, encryptedRequestId, StringComparison.InvariantCultureIgnoreCase)
                .Replace(emailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase);
            await PasswordResetBody(siteUrl, userFirstName, encryptedRequestId, emailAddress);
        }
        private async Task<string> PasswordResetBody(string siteUrl, string userFirstName, string PasswordResetRequestId, string emailAddress)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-a9c768abff9147bb92ecf9c48ead2f83");
                var templateData = new
                {
                    siteUrl,
                    userFirstName,
                    PasswordResetRequestId,
                    subject
                };
                var response = await SendEmail(subject, templateData, emailAddress);
                if (response)
                {
                    return "Email sent successfully!";
                }
                else
                {
                    throw new Exception("Failed to send email.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<bool> SendEmail(string subject, object templateData, string to)
        {
            try
            {
                var templatedata = templateData;
                string apiKey = "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";
                var client = new SendGrid.SendGridClient(apiKey);
                var from = new EmailAddress("liam.geoghegan@esarisk.com", "Admin");
                var toList = new EmailAddress(to);
                var templateId = "d-a9c768abff9147bb92ecf9c48ead2f83";

                var msg = MailHelper.CreateSingleTemplateEmail(from, toList, templateId, templatedata);
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return true;
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
