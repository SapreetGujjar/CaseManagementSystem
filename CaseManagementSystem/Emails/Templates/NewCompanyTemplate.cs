using CaseManagementSystem.Helpers;
using Microsoft.AspNetCore.Components.Routing;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace CaseManagementSystem.Emails.Templates
{
    public class NewCompanyTemplate:BaseEMailTemplate
    {
        public string CompanyName = "[CompanyName]";
        public string emailAddress { get; } = "[emailAddress]";
        public string ccEmail { get; } = "[ccEmail]";
        private string subject = @"Company Account created";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }
        private string body = "";
        public async Task Initialize(string siteUrl, string companyName, string emailAddress, string ccEmail)
        {
            CryptoService cs = new CryptoService();
            body = Body.Replace("[siteUrl]", siteUrl, StringComparison.InvariantCultureIgnoreCase)
                              .Replace("[emailAddress]", emailAddress, StringComparison.InvariantCultureIgnoreCase)
                              .Replace("[ccEmail]", ccEmail, StringComparison.InvariantCultureIgnoreCase)
                              .Replace("[CompanyName]", companyName, StringComparison.InvariantCultureIgnoreCase);

            //body += "<br/><br/>" + this.footer;
            await CompanyBody(companyName, emailAddress, ccEmail);
        }
        private async Task<string> CompanyBody(string companyName, string emailAddress, string ccEmail)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-4f1162a00a884e30864f9f5f2284ef65");
                var templateData =new
                {
                    companyName,
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
                var templateId = "d-4f1162a00a884e30864f9f5f2284ef65"; 

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

    }
}
