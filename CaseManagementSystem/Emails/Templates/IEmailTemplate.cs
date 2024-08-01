namespace CaseManagementSystem.Emails.Templates
{
    public interface IEmailTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
