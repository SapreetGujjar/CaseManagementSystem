using CaseManagementSystem.Emails.Templates;
using CaseManagementSystem.Pages;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;

namespace CaseManagementSystem.Emails
{
    public class EmailServiceMock : IEmailService
    {
        private string _adminEmail, _from, _smtpServer, _userName, _password, _ccEmail;
        private int _port;
        public EmailServiceMock(string adminEmail, string from, string smtpServer, int port, string userName, string password, string ccEmail)
        { 
            _adminEmail = adminEmail;
            _from = from;
            _smtpServer = smtpServer;
            _userName = userName;
            _password = password;
            _port = port;
            _ccEmail = ccEmail;
        }

        public async Task SendEMailNotificationToAdminAsync(IEmailTemplate notification)
        {
            Console.WriteLine(notification.Subject);
        }

        public async Task SendEmailNotificationAsync(IEmailTemplate notification, string to, string cc)
        {
            Console.WriteLine(notification.Subject);
        }
        public async Task SendEmailNotificationAsync(IEmailTemplate notification, List<string> to, List<string> cc)
        {
            Console.WriteLine(notification.Subject);
        }
    }
}
