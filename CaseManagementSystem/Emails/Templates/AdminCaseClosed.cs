//Sent to an agent when a case is assigned to them.
using CaseManagementSystem.Helpers;

namespace CaseManagementSystem.Emails.Templates
{
    public class AdminCaseClosed : BaseEMailTemplate, IEmailTemplate
    {
        public AdminCaseClosed(string siteUrl, string caseNumber, string caseId) {
            CryptoService cs = new CryptoService();

            string caseUrl = "<a clicktracking= 'off' href='" + siteUrl + "/login/" + cs.EncryptStringEscaped(caseId) + "'>" + caseNumber + "</a>";

            subject = subject.Replace("[Case Id]", caseNumber, StringComparison.InvariantCultureIgnoreCase);
            body = body
                .Replace("[case ID - link to case record]", caseUrl, StringComparison.InvariantCultureIgnoreCase);

            body += "<br/><br/>" + this.footer;
        }
        private string subject = @"Case [case ID] closed";
        private string body = @"Case [case ID - link to case record] has been completed and closed.";

        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }
    }
}