// Sent to the client when a case is created.
using CaseManagementSystem.Pages;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;

namespace CaseManagementSystem.Emails.Templates
{
    
    public class UserPasswordResetCompleted: BaseEMailTemplate, IEmailTemplate
    {
        public string FirstName = "[First name]";
        public string emailAddress { get; } = "[emailAddress]";
        public string ccEmail { get; } = "[ccEmail]";
        public async Task Initialized(string userFirstName, string emailAddress)
        {
         body = body
            .Replace(emailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase)
                 .Replace(ccEmail, ccEmail, StringComparison.InvariantCultureIgnoreCase)
                .Replace(FirstName, userFirstName, StringComparison.InvariantCultureIgnoreCase);
            body += "<br/><br/>" + this.footer;
            await UserPasswordBody( userFirstName, emailAddress, ccEmail);
        }
        private async Task<string> UserPasswordBody(string userFirstName, string emailAddress, string ccEmail)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-2fa9ce8ff9a3434e8e1646bc727e255c");
                var templateData = new
                {
                    userFirstName,
                    subject
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
                var templateId = "d-2fa9ce8ff9a3434e8e1646bc727e255c";

                var msg = MailHelper.CreateSingleTemplateEmail(from, toList, templateId, templatedata);
                msg.Subject = subject;
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

        private string subject = @"Password reset completed";
        private string body = "";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }

    }
}
