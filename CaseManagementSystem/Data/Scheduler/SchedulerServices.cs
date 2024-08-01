using CaseManagementSystem.Data.Users;
using CaseManagementSystem.Emails;
using CaseManagementSystem.Emails.Templates;
using CMS.DL.DM;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;



namespace CaseManagementSystem.Data.Scheduler
{
    public class SchedulerServices
    {
        private readonly string _connectionString = "";

        private readonly ILogger<SchedulerServices> _logger;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IEmailService _EMailService;
        private readonly UsersService _UsersService;
        private readonly IConfiguration _configuration; 


        public SchedulerServices(IConfiguration configuration,ILogger<SchedulerServices> logger, IRecurringJobManager recurringJobManager,IEmailService EMailService, UsersService UsersService)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            _logger = logger;
            _recurringJobManager = recurringJobManager;
        _EMailService = EMailService;
            _UsersService = UsersService;
            _configuration = configuration;
        }
 
        public SchedulerServices(string connection, IConfiguration configuration, IRecurringJobManager recurringJobManager)
        {
            _logger.LogInformation("testtt>>>>");
        }
        public async Task Initialize()
        {
           _logger.LogInformation("Initializing job...");

            //var jobId = Guid.NewGuid().ToString();
            //_recurringJobManager.AddOrUpdate(jobId, () => CustomRecurringJob(jobId),
            //    "0/10 * * * * *"); // Use "0/10 * * * * *" instead of "*/10 * * * * *"

            //_logger.LogInformation("Job scheduled successfully.");


        }

        public Task CustomRecurringJob()
        {
            _logger.LogInformation($"Executing Job Id: at {DateTime.Now}");
            return Task.CompletedTask;
        }
        public async Task<List<UsersViewModel>> GetUsersPendingDays()
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            List<CMS.DL.Model.Users> users = await usersDM.GetUsersDocument();
            DateTime today = DateTime.Today;
            foreach (var user in users)
            {
                var DaysType = "";
                if (user.NumberOfDays == "30" || user.NumberOfDays == "15" || user.NumberOfDays == "7" || user.NumberOfDays == "1")
                {
                    ExpiredDocumentsNotification notification = new ExpiredDocumentsNotification();
                      await notification.Initialized(user.NumberOfDays, user.FirstName, user.EmailAddress, _configuration.GetValue<string>("SiteUrl"));
                    //await _EMailService.SendEmailNotificationAsync(notification, user.EmailAddress, user.ccEmail);
                     DaysType = user.NumberOfDays == "30" ? "Monthly" :
                        user.NumberOfDays == "15" ? "Partially" : 
                        user.NumberOfDays == "7" ? "Weekly" : "Day";
                    
                   
                        await _UsersService.UserDocumentNotificationSave(user.Id, "Pending", today , today, DaysType);
                }else if(user.NumberOfDays == "0")
                {
                    bool IsActive = false;
                    DaysType = "Day";
                    await _UsersService.UpdateUsersActive(user.Id, IsActive);
                    await _UsersService.UserDocumentNotificationSave(user.Id, "Expired", today, today, DaysType);
                }

            }
            return null;
        }


        //public void GetUsersPendingDayss()
        //{

        //}
    }
}
