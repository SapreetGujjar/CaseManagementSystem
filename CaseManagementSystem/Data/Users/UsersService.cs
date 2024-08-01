using CMS.DL.DM;
using System.Runtime.CompilerServices;
using CaseManagementSystem.Data;
using CaseManagementSystem.Emails.Templates;
using CaseManagementSystem.Pages;
using CaseManagementSystem.Data.Subjects;

namespace CaseManagementSystem.Data.Users
{
    public class UsersService
    {
        private readonly string _connectionString = "";

        public UsersService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        #region GET

        public async Task<IEnumerable<UsersViewModel>> GetAllUsersAsync()
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            IEnumerable<CMS.DL.Model.Users> users = await usersDM.GetAllUsersAsync();
            return users.Select(u => u.ToVM());
        }

        public async Task<IEnumerable<UsersViewModel>> GetAllUsersByClientAsync(Guid? clientId)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            IEnumerable<CMS.DL.Model.Users> users = await usersDM.GetAllUsersByClientAsync(clientId);

            return users.Select(u => u.ToVM());
        }
        public async Task<IEnumerable<UsersViewModel>> GetAllAgentsAsync()
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            IEnumerable<CMS.DL.Model.Users> users = await usersDM.GetAllAgentsAsync();

            return users.Select(u => u.ToVM());
        }

        public async Task<IEnumerable<UsersViewModel>> GetAllClientsAsync()
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            IEnumerable<CMS.DL.Model.Users> users = await usersDM.GetAllClientsAsync();

            return users.Select(u => u.ToVM());
        }

        public async Task<UsersViewModel> GetUsersByIdAsync(Guid id)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            CMS.DL.Model.Users user = await usersDM.GetUsersByIdAsync(id);

            if (user != null)
            {
                return user.ToVM();
            }
            else
            {
                return null;
            }
        }

        public async Task<UsersViewModel> GetUsersByUserNameAsync(string userName, Guid? excludeId = null)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            CMS.DL.Model.Users user = await usersDM.GetUsersByUserNameAsync(userName, excludeId);

            if (user != null)
            {
                return user.ToVM();
            }
            else
            {
                return null;
            }
        }

        public async Task<UsersViewModel> GetUsersByEmailAddressAsync(string emailAddress, Guid? excludeId = null)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            CMS.DL.Model.Users user = await usersDM.GetUsersByEmailAddressAsync(emailAddress, excludeId);

            if (user != null)
            {
                return user.ToVM();
            }
            else
            {
                return null;
            }
        }

        public async Task<UsersViewModel> CheckLogin(string userName)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            CMS.DL.Model.Users user = await usersDM.CheckLogin(userName);

            if (user != null)
            {
                return user.ToVM();
            }
            else
            {
                return null;
            }
        }
    

        public async Task<List<UserTelephoneViewModel>> GetUsersTelephoneViewModel(Guid id)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            List<CMS.DL.Model.UserTelephone> users = await usersDM.GetUsersTelephoneNumber(id);
            List<UserTelephoneViewModel> userViewModels = users.ToVM();
            return userViewModels;

        }

        public async Task<UsersViewModel> GetLastUsersAsync()
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            CMS.DL.Model.Users user = await usersDM.GetLastUsersAsync();

            if (user != null)
            {
                return user.ToVM();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region INSERT/UPDATE/DELETE
        public async Task<int> InsertUsersAsync(UsersViewModel usersView)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            int result = await usersDM.InsertUsersAsync(usersView.ToModel());
            return result;
        }

        public async Task<int> UpdateUsersAsync(UsersViewModel usersView)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            int result = await usersDM.UpdateUsersAsync(usersView.ToModel());

            return result;
        }

        public async Task<int> UpdateUsersActive(Guid Id,bool IsActive)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            int result = await usersDM.UpdateUsersActive(Id, IsActive);

            return result;
        }

        public async Task<int> ActiveUsersAsync(Guid id, bool isActive, Guid updatedBy)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            int result = await usersDM.ActiveUsersAsync(id, isActive, updatedBy);

            return result;
        }

        public async Task<int> UpdateUsersRoleTypeAsync(Guid id, byte? roleType, Guid updatedBy)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            int result = await usersDM.UpdateUsersRoleTypeAsync(id, roleType, updatedBy);

            return result;
        }

        public async Task<int> UpdateUsersPasswordAsync(Guid id, string password, Guid updatedBy)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            int result = await usersDM.UpdateUsersPasswordAsync(id, password, updatedBy);

            return result;
        }

        public async Task<int> UpdateUsersLastLoginAsync(Guid id)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            int result = await usersDM.UpdateUsersLastLoginAsync(id);

            return result;
        }

        public async Task<Guid> RequestResetPasswordAsync(string email)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            Guid result = await usersDM.RequestResetPasswordAsync(email);

            return result;

        }
        public async Task ResetPasswordAsync(Guid userPasswordResetRequestId, string password)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            await usersDM.ResetPassword(userPasswordResetRequestId, password);
        }
        public async Task UserDocumentNotificationSave(Guid Id, string Status,DateTime Created,DateTime Updated, string DaysType)
        {
            UsersDM usersDM = new UsersDM(_connectionString);
            await usersDM.UserDocumentNotificationSave(Id ,Status ,Created ,Updated ,DaysType);
        }
        #endregion
    }
}
