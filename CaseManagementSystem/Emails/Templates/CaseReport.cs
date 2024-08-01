// Sent to the client when the case is reported.
using SendGrid.Helpers.Mail;
using System.Net;

namespace CaseManagementSystem.Emails.Templates
{
    public class CaseReport : BaseEMailTemplate, IEmailTemplate
    {
        private string subject = "[Case ID] Report";
        private string body = "";

        public string caseNumber = "[Case ID]";
        public string caseResultSummary = "[caseResultSummary]";
        public string clientName = "[subjectName]";
        public string subjectName = "[subjectName]";
        public string providedInformation = "[providedInformation]";

        public async Task Initialize(string clientName, string caseNumber, string subjectName, string caseResultSummary, string providedInformation, string emailAddress, string ccEmail)
        {
            subject = subject.Replace("[Case ID]", caseNumber, StringComparison.InvariantCultureIgnoreCase);
            body = body
                .Replace(caseNumber, caseNumber, StringComparison.InvariantCultureIgnoreCase)
                .Replace(clientName, clientName, StringComparison.InvariantCultureIgnoreCase)
                .Replace(subjectName, subjectName, StringComparison.InvariantCultureIgnoreCase)
                .Replace(caseResultSummary, caseResultSummary, StringComparison.InvariantCulture)
                .Replace(emailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase)
                .Replace(ccEmail, ccEmail, StringComparison.InvariantCultureIgnoreCase)
                .Replace(providedInformation, providedInformation, StringComparison.InvariantCulture);
            await CaseReportBody(caseNumber, clientName, subjectName, caseResultSummary, providedInformation, emailAddress, ccEmail);
        }

        private async Task<string> CaseReportBody(string caseNumber, string clientName, string subjectName, string caseResultSummary, string providedInformation, string emailAddress, string ccEmail)
        {
            var msg = new SendGridMessage();
            msg.SetTemplateId("d-3067b28155104f9b8d877d5f7a2018f7");
            var templateData = new
            {
               
                clientName,
                caseNumber,
                caseResultSummary,
                providedInformation,
                subjectName,
                subject
            };
            var response = await SendEmail(subject, templateData, emailAddress, new List<string> { ccEmail });
            return response ? "Email sent successfully!" : throw new Exception("Failed to send email.");
        }
        private async Task<bool> SendEmail(string subject, object templateData, string to, List<string> cc)
        {
            var templatedata = templateData;
            string apiKey = "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";
            var client = new SendGrid.SendGridClient(apiKey);
            var from = new EmailAddress("liam.geoghegan@esarisk.com", "Admin");
            var toList = new EmailAddress(to);
            var templateId = "d-3067b28155104f9b8d877d5f7a2018f7";
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
        //        private string body = @"
        //Hi[First name],<br/><br/>
        //Please find enclosed our report in respect of[case ID] - [Subject name].<br/><br/>
        //[caseResultSummary]<br/>
        //Provided information:<br/><br/>
        //[providedInformation]<br/><br/>
        //We trust that we have been of assistance to you and thank you for using our services.<br/><br/>
        //Kind regards<br/><br/>
        //ESA Risk";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }

    }
}
