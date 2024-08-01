// Sent to an agent following a decision of approve or reject by the Internaladmin.
using CaseManagementSystem.Data.Documents;
using CaseManagementSystem.Helpers;
using CMS.DL.Model;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace CaseManagementSystem.Emails.Templates
{
    public class CaseApprovedOrRejected: BaseEMailTemplate
    {
        public async Task Initialized(string siteUrl, string caseNumber, string caseId, string agentName, string notes, bool isApproved, List<DocumentViewModel> documents, string emailAddress, string ccEmail)
        {
            var AttachmentHtml = "";
            foreach (var file in documents)
            {
                AttachmentHtml += file.FileName + ",";
            }
            if (AttachmentHtml.Length > 0)
            {
                AttachmentHtml = AttachmentHtml.Remove(AttachmentHtml.Length - 1);
            }
            subject = subject.Replace("[Case ID]", caseNumber);
            CryptoService cs = new CryptoService();

            string caseUrl = "<a clicktracking= 'off' href='" + siteUrl + "/login/" + cs.EncryptStringEscaped(caseId) + "'>" + caseNumber + "</a>";

            if (isApproved)
            {
                subject = subject.Replace("[decision - approved OR rejected]", "APPROVED");
                body = bodyApproved
                    .Replace("[First name]", agentName, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("[Case ID]", caseNumber, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("[emailAddress]", emailAddress, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("[ccEmail]", ccEmail, StringComparison.InvariantCultureIgnoreCase)
                  .Replace("[Attachments]", AttachmentHtml, StringComparison.InvariantCultureIgnoreCase);
                await CaseApproveBody(siteUrl, caseNumber, documents, isApproved, notes, agentName, emailAddress, ccEmail);
            }
            else
            {
                subject = subject.Replace("[decision - approved OR rejected]", "REJECTED");
                body = bodyRejected
                    .Replace("[Case ID - link to case record]", caseUrl, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("[First name]", agentName, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("[Notes from Admin]", notes, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("[Attachments]", AttachmentHtml, StringComparison.InvariantCultureIgnoreCase);
                await CaseApproveBody(siteUrl, caseNumber, documents, isApproved, notes, agentName, emailAddress, ccEmail); 
            }

            body += "<br/><br/>" + this.footer;
        }
        private async Task<string> CaseApproveBody(string siteUrl, string caseNumber, List<DocumentViewModel> documents, bool isApproved,string notes, string agentName, string emailAddress, string ccEmail)
        {
            try
            {
                var msg = new SendGridMessage();
                if (isApproved)
                {
                    msg.SetTemplateId("d-4215ba495e944571972a8dd7f6158958");
                    var templateData = new
                    {
                        siteUrl,
                        agentName,
                        caseNumber,
                        subject
                    };
                    var response = await SendEmail(subject, isApproved, templateData, emailAddress, new List<string> { ccEmail },documents);
                    return response ? "Email sent successfully!" : throw new Exception("Failed to send email.");
                }
                else
                {
                    msg.SetTemplateId("d-3f9c404cc3f145c190ec59acf6b0675e");
                    var templateData = new
                    {
                        siteUrl,
                        notes,
                        caseNumber,
                        subject
                    };
                    var response = await SendEmail(subject, isApproved, templateData, emailAddress, new List<string> { ccEmail }, documents);
                    return response ? "Email sent successfully!" : throw new Exception("Failed to send email.");
                }
               
             
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> SendEmail(string subject, bool isApproved ,object templateData, string to, List<string> cc, List<DocumentViewModel> documents)
        {
            try
            {
                var templatedata = templateData;
                var templateId = "";
                string apiKey = "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";
                var client = new SendGrid.SendGridClient(apiKey);
                var from = new EmailAddress("liam.geoghegan@esarisk.com", "Admin");
                var toList = new EmailAddress(to);
                if(isApproved)
                {
                     templateId = "d-4215ba495e944571972a8dd7f6158958";
                }
               else
                {
                     templateId = "d-3f9c404cc3f145c190ec59acf6b0675e";
                }

                var msg = MailHelper.CreateSingleTemplateEmail(from, toList, templateId, templatedata);
                msg.Subject = subject;
                foreach (var document in documents)
                {
                    var fileContent = Convert.ToBase64String(document.Data);
                    msg.AddAttachment(document.FileName, fileContent);
                }
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
        private string subject = @"[Case ID] update [decision - approved OR rejected]";
        private string body = string.Empty;
        private string bodyApproved = "";
        private string bodyRejected = "";
    }
}
