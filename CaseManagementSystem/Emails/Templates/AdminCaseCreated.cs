//Sent to an agent when a case is assigned to them.
using CaseManagementSystem.Helpers;

namespace CaseManagementSystem.Emails.Templates
{
    public class AdminCaseCreated: BaseEMailTemplate, IEmailTemplate
    {
        public AdminCaseCreated(string siteUrl, string caseNumber, string caseId, string createdByUser, string forCompany)
        {
            CryptoService cs = new CryptoService();

            string caseUrl = "<a clicktracking= 'off' href='" + siteUrl + "/login/" + cs.EncryptStringEscaped(caseId) + "'>" + caseNumber + "</a>";

            subject = subject
                .Replace("[Case ID]", caseNumber, StringComparison.InvariantCultureIgnoreCase);
            
            body = body.Replace("[User name]", createdByUser, StringComparison.InvariantCultureIgnoreCase)
                .Replace("[Client company]", forCompany, StringComparison.InvariantCultureIgnoreCase)
                .Replace("[Case ID - link to case record]", caseUrl, StringComparison.InvariantCultureIgnoreCase);

            body += "<br/><br/>" + this.footer;
        }
        private string subject = @"New case created - [Case ID]";
        private string body = @"
            [User name] has created a new case for [Client company].<br/><br/>
            View case [Case ID - link to case record]";

        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }
    }
}
