using MimeKit;
using SendGrid.Helpers.Mail;
using System.Net;

namespace CaseManagementSystem.Emails.Templates
{
    public class CaseWatcherApprovedEmail : BaseEMailTemplate, IEmailTemplate
    {
        
        private string subject = @"Subject has been traced.";
        public async Task Initialize(string SubjectName,string emailAddress, string ccEmail)
        {
            body = body
                .Replace("[SubjectName]", SubjectName, StringComparison.InvariantCultureIgnoreCase);
            await CaseWatcherApprovedBody(SubjectName, emailAddress, ccEmail);
        }
        private async Task<string> CaseWatcherApprovedBody(string SubjectName, string emailAddress, string ccEmail)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-951ec422b09b4909853b17989b1850be");
                var templateData = new
                {
                    SubjectName,
                    subject
                };
                var response = await SendEmail(subject, templateData, emailAddress, new List<string> { ccEmail });
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
                var templateId = "d-951ec422b09b4909853b17989b1850be";

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
        //private string GetExpiryDocumentTemplate()
        //{
        //    var pathToFile = Path.Combine(
        //    Directory.GetCurrentDirectory(), "wwwroot") +
        //     Path.DirectorySeparatorChar.ToString()
        //     + "Templates"
        //     + Path.DirectorySeparatorChar.ToString()
        //     + "CaseWatcherApprovedTemplate.html";
        //    var builder = new BodyBuilder();
        //    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
        //    {
        //        builder.HtmlBody = SourceReader.ReadToEnd();
        //    }
        //    return builder.HtmlBody.ToString();
        //}

        private string body = "";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }
    }
}
