//Sent to an agent when a case is assigned to them.
using CaseManagementSystem.Helpers;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace CaseManagementSystem.Emails.Templates
{
    public class CaseAssigned: BaseEMailTemplate, IEmailTemplate
    {
        private string subject = @"New case assigned - [Case ID]";
        public string CaseId = "[Case ID]";
        public string FirstName = "[First name]";
        public string emailAddress { get; } = "[emailAddress]";
        public string ccEmail { get; } = "[ccEmail]";
        public async Task Initialized(string siteUrl, string caseNumber, string caseId, string agentName, string emailAddress, string ccEmail)
        {
            CryptoService cs = new CryptoService();
            string caseUrl = "<a clicktracking= 'off' href='" + siteUrl + "/login/" + cs.EncryptStringEscaped(caseId) + "'>" + caseNumber + "</a>";
            subject = subject.Replace("[Case ID]", caseNumber);
            body = Body
                .Replace(CaseId, caseUrl, StringComparison.InvariantCultureIgnoreCase)
                .Replace(emailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase)
                 .Replace(ccEmail, ccEmail, StringComparison.InvariantCultureIgnoreCase)
                .Replace(FirstName, agentName, StringComparison.InvariantCultureIgnoreCase);
            body += "<br/><br/>" + this.footer;
            await CaseBody(siteUrl, caseNumber, caseId, agentName, emailAddress, ccEmail);
        }
        private async Task<string> CaseBody(string siteUrl, string caseId, string caseNumber, string agentName, string emailAddress, string ccEmail)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-9b66518a0a1942409c31f7b482b3fec5");
                var templateData = new
                {
                    siteUrl,
                    agentName,
                    caseNumber,
                    caseId,
                    subject
                };
                var response = await SendEmail(Subject, templateData, emailAddress, new List<string> { ccEmail });
                return response ? "Email sent successfully!" : throw new Exception("Failed to send emai l.");
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
                var templateId = "d-9b66518a0a1942409c31f7b482b3fec5";

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

        private string body = "";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }

    }
}
