// Sent to the client when a case is created.
using CaseManagementSystem.Pages;
using MimeKit;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail.Model;
using System.Net.Mail;
using System.Linq;
using Blazor.Component.Maps;

namespace CaseManagementSystem.Emails.Templates
{
    public class CaseCreated : BaseEMailTemplate, IEmailTemplate
    {

        public string CaseId = "[Case ID]";
        public string Fee = "[Fee]";
        public string FirstName = "[First name]";
        public string SubjectName = "[Subject's name]";
        public string emailAddress { get; } = "[emailAddress]";
        public string ccEmail { get; } = "[ccEmail]";
        private string subject = @"New case Created";
        //private string subject = "";
        private string body = "";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }

        #region
        public async Task Initialize(string siteUrl, string clientName, string caseNumber, string fee, string subjectName, string emailAddress, string ccEmail)
        {
            body = Body.Replace("[siteUrl]", siteUrl, StringComparison.InvariantCultureIgnoreCase)
           .Replace(CaseId, caseNumber, StringComparison.InvariantCultureIgnoreCase)
           .Replace(Fee, fee, StringComparison.InvariantCultureIgnoreCase)
           .Replace(FirstName, clientName, StringComparison.InvariantCultureIgnoreCase)
           .Replace(emailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase)
           .Replace(ccEmail, ccEmail, StringComparison.InvariantCultureIgnoreCase)
           .Replace(SubjectName, subjectName, StringComparison.InvariantCultureIgnoreCase);
            body = await CaseBody(siteUrl, clientName, caseNumber, fee, subjectName, emailAddress, ccEmail);
        }


        //body of email which is containing siteUrl,clientName,caseNumber,fee as params
        //email address details subjectName,ccEmail,emailAddress

        private async Task<string> CaseBody(string siteUrl, string clientName, string caseNumber, string fee, string subjectName, string emailAddress, string ccEmail)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetTemplateId("d-c599b0ab5e8b40ee87d89c988c915350");
                var templateData = new
                {
                    siteUrl,
                    clientName,
                    caseNumber,
                    fee,
                    subjectName,
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

        //function for send email using Api Key , from and template Id
        private async Task<bool> SendEmail(string subject, object templateData, string to, List<string> cc)
        {
            try
            {
                var templatedata = templateData;
                string apiKey = "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";
                var client = new SendGrid.SendGridClient(apiKey);
                var from = new EmailAddress("liam.geoghegan@esarisk.com", "Admin");
                var toList = new EmailAddress(to);
                var templateId = "d-c599b0ab5e8b40ee87d89c988c915350";

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




        
        #endregion


    }

}
