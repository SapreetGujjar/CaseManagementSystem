using MimeKit;

namespace CaseManagementSystem.Emails.Templates
{
    public class SendEmailCCTemplate : BaseEMailTemplate, IEmailTemplate
    {
        private string subject = @"Email sent to New User.";
        public SendEmailCCTemplate(string FirstName)
        {
            body = GetCCEmailTemplate()
                .Replace("[FirstName]", FirstName, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetCCEmailTemplate()
        {
            var pathToFile = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot") +
             Path.DirectorySeparatorChar.ToString()
             + "Templates"
             + Path.DirectorySeparatorChar.ToString()
             + "CCEmailTemplate.html";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            return builder.HtmlBody.ToString();
        }

        private string body = "";
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }
    }
}
