/*
 If a case is checked out by a user, and the user who has the case checked out checks it in / runs out of time and doesn't provide a case update, the case becomes open to other agents to work on. An agent should be able to ask to 'watch' a case that is checked out. If that case becomes available to work on again, they should be sent a notification email.
*/
using CaseManagementSystem.Helpers;

namespace CaseManagementSystem.Emails.Templates
{
    public class CaseWatched: BaseEMailTemplate, IEmailTemplate
    {
        public CaseWatched(string siteUrl, string caseNumber, string caseId, string watcherName)
        {
            CryptoService cs = new CryptoService();

            string caseUrl = "<a clicktracking= 'off' href='" + siteUrl + "/login/" + cs.EncryptStringEscaped(caseId) + "'>" + caseNumber + "</a>";

            subject = subject.Replace("[Case ID]", caseNumber, StringComparison.InvariantCultureIgnoreCase);
            body = body
                .Replace("[First name]", watcherName, StringComparison.InvariantCultureIgnoreCase)
                .Replace("[Case ID - link to case record]", caseUrl, StringComparison.InvariantCultureIgnoreCase)
                .Replace("[Case ID]", caseNumber, StringComparison.InvariantCultureIgnoreCase);
            body += "<br/><br/>" + this.footer;
        }
        private string subject = @"[Case ID] now available";
        private string body = @"
Hi [First name],<br/><br/>
Case [case ID] is now open again.<br/>
Check out [Case ID - link to case record] to work on this case / provide a case update.<br/>

Kind regards<br/>

ESA Risk";

        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }

    }
}
