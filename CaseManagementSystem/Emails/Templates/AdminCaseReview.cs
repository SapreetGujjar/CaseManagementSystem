//Sent to an agent when a case is assigned to them.
using CaseManagementSystem.Helpers;

namespace CaseManagementSystem.Emails.Templates
{
    public class AdminCaseReview: BaseEMailTemplate, IEmailTemplate
    {
        public AdminCaseReview(string siteUrl, string caseNumber, string caseId, string agentName)
        {
            CryptoService cs = new CryptoService();

            string caseUrl = "<a clicktracking= 'off' href='" + siteUrl + "/login/" + cs.EncryptStringEscaped(caseId) + "'>" + caseNumber + "</a>";

            subject = subject.Replace("[Case ID]", caseNumber, StringComparison.InvariantCultureIgnoreCase);
            body = body.Replace("[Agent name]", agentName, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("[case ID - link to case record]", caseUrl, StringComparison.InvariantCultureIgnoreCase);
            body += "<br/><br/>" + this.footer;
        }
        private string subject = @"Review case update on [case ID]";
        private string body = @"
[Agent name] has submitted a case update on [case ID - link to case record].<br/><br/>

Please approve or reject the update.";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }

    }
}
