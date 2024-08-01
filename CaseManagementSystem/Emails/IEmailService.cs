using CaseManagementSystem.Emails.Templates;
using CaseManagementSystem.Pages;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;

namespace CaseManagementSystem.Emails
{
    public interface IEmailService
    {
        Task SendEMailNotificationToAdminAsync(IEmailTemplate notification);
        Task SendEmailNotificationAsync(IEmailTemplate notification, string to, string cc);
        Task SendEmailNotificationAsync(IEmailTemplate notification, List<string> to, List<string> cc);
    }
}
