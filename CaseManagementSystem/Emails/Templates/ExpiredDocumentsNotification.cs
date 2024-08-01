using CaseManagementSystem.Pages;
using MimeKit;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;


namespace CaseManagementSystem.Emails.Templates
{
    public class ExpiredDocumentsNotification : BaseEMailTemplate, IEmailTemplate
    {
        private string subject = @"Document Expiry Reminder";
        public async Task Initialized(string NoOfDays, string FirstName, string emailAddress,string siteUrl)
        {
            //body = GetExpiryDocumentTemplate()
            //.Replace("[NoOfDays]", NoOfDays, StringComparison.InvariantCultureIgnoreCase)
            //.Replace("[First name]", FirstName, StringComparison.InvariantCultureIgnoreCase)
            //.Replace("[siteUrl]", siteUrl, StringComparison.InvariantCultureIgnoreCase);
            body = body
            .Replace("[NoOfDays]", NoOfDays, StringComparison.InvariantCultureIgnoreCase)
            .Replace("[First name]", FirstName, StringComparison.InvariantCultureIgnoreCase)
            .Replace(emailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase)
            .Replace("[siteUrl]", siteUrl, StringComparison.InvariantCultureIgnoreCase);
            await ExpiryDocumentBody(NoOfDays, FirstName, emailAddress, siteUrl);
        }

        private async Task<string> ExpiryDocumentBody(string NoOfDays, string FirstName, string emailAddress, string siteUrl)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-e3d2f3fbe9274c839bef7f82ce711d5c");
                var templateData = new
                {
                    siteUrl,
                    NoOfDays,
                    FirstName,
                    subject
                };
                var response = await SendEmail(Subject, templateData, emailAddress);
                return response ? "Email sent successfully!" : throw new Exception("Failed to send emai l.");
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
                var templateId = "d-e3d2f3fbe9274c839bef7f82ce711d5c";
                var msg = MailHelper.CreateSingleTemplateEmail(from, toList, templateId, templatedata);
                msg.Subject = subject;
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
        private string GetExpiryDocumentTemplate()
        {
            var pathToFile = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot") +
             Path.DirectorySeparatorChar.ToString()
             + "Templates"
             + Path.DirectorySeparatorChar.ToString()
             + "ExpiredDocumentTemplate.html";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            return builder.HtmlBody.ToString();
        }


        private string body = "";
        public string Subject { get => subject; set => subject = value; }

        public string Body { get => body; set => body = value; }
    }
}
