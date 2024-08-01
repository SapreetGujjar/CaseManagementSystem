using CMS.DL.Model;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CMS.DL.DM
{
    public class UsersDM
    {
        private readonly IDbConnection _dbConnection;

        public object UserId { get; private set; }

        public UsersDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        #region GET

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            string sql = "SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id";
            return await _dbConnection.QueryAsync<Users>(sql);
        }
        public async Task<IEnumerable<Users>> GetAllUsersByClientAsync(Guid? clientId)
        {
            string sql = "SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE U.CreatedBy = @CreatedBy";
            return await _dbConnection.QueryAsync<Users>(sql, new { CreatedBy = clientId });
        }

        public async Task<IEnumerable<Users>> GetAllAgentsAsync()
        {
            string sql = "SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE U.RoleType IN (3, 4)";
            return await _dbConnection.QueryAsync<Users>(sql);
        }

        public async Task<IEnumerable<Users>> GetAllClientsAsync()
        {
            string sql = "SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE U.RoleType = 2";
            return await _dbConnection.QueryAsync<Users>(sql);
        }

        public async Task<Users> GetUsersByIdAsync(Guid id)
        {
            try
            {
                string sql = $@"SELECT U.*, C.CompanyName FROM Users AS U
                                LEFT JOIN Companies AS C ON U.CompanyId = C.Id
                                WHERE U.Id = @Id";
                return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql, new { Id = id });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Users> GetUsersByUserNameAsync(string userName, Guid? excludeId = null)
        {
            if (excludeId != null && excludeId != Guid.Empty)
            {
                string sql = $"SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE U.UserName = @UserName AND U.Id NOT IN (@Id)";
                return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql, new { UserName = userName, Id = excludeId });
            }
            else
            {
                string sql = $"SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE U.UserName = @UserName";
                return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql, new { UserName = userName });
            }
        }

        public async Task<Users> GetUsersByEmailAddressAsync(string emailAddress, Guid? excludeId = null)
        {
            if (excludeId != null && excludeId != Guid.Empty)
            {
                string sql = $"SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE U.EmailAddress = @EmailAddress AND U.Id NOT IN (@Id)";
                return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql, new { EmailAddress = emailAddress, Id = excludeId });
            }
            else
            {
                string sql = $"SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE U.EmailAddress = @EmailAddress";
                return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql, new { EmailAddress = emailAddress });
            }
        }

        public async Task<Users> CheckLogin(string userName)
        {
            string sql = $"SELECT U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id WHERE (U.UserName = @UserName OR U.EmailAddress = @UserName) AND U.RoleType IS NOT NULL AND U.IsActive = 1";
            return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql, new { UserName = userName });
        }

        public async Task<List<UserTelephone>> GetUsersTelephoneNumber(Guid Id)
        {
            try
            {
                string sql = @"SELECT * FROM UserTelephones WHERE CreatedBy = @Id";
                var list = await _dbConnection.QueryAsync<UserTelephone>(sql, new { Id = Id });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }

        }

        public async Task<IQueryable<Companies>> GetAllCompaniesAsync()
        {
            string sql = "SELECT * FROM Companies";
            var companies = await _dbConnection.QueryAsync<Companies>(sql);
            return companies.AsQueryable();
        }

        public async Task<Users> GetLastUsersAsync()
        {
            string sql = $"SELECT TOP 1 U.*, C.CompanyName FROM Users AS U LEFT JOIN Companies AS C ON U.CompanyId = C.Id ORDER BY U.Created DESC";
            return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql);
        }

        public async Task<List<Users>> GetUsersDocument()
        {
            string sql = $"declare @TodayDate date = Getutcdate();\r\nSELECT u.FirstName,u.EmailAddress,DATEDIFF(day, @TodayDate, ud.ExpiryDate) AS NumberOfDays, *, GETDATE() AS NewUpdated FROM UserDocuments ud left join Users u on u.Id = ud.CreatedBy WHERE DATEDIFF(day, @TodayDate, ud.ExpiryDate) <= 30 and FileType  = 1";
            return (await _dbConnection.QueryAsync<Users>(sql)).ToList();
        }

        public async Task<Users> UserDocumentNotificationSave(Guid Id, string Status, DateTime Created, DateTime Updated, string DaysType)
        {
            string sql = $"insert into Notifications(UserId,Status,Created,Updated,DaysType)\r\nvalues(@UserId,@Status, @Created,@Updated,@DaysType)";
            return await _dbConnection.QueryFirstOrDefaultAsync<Users>(sql, new { UserId = Id, Status = Status, Created = Created, Updated = Updated, DaysType = DaysType });
        }
        #endregion

        #region Reset Password

        public async Task<Guid> RequestResetPasswordAsync(string email)
        {
            Guid guid = Guid.NewGuid();

            Users user = await this.GetUsersByEmailAddressAsync(email);
            if (user == null)
                throw new Exception("Invalid email address");

            string sql = "INSERT INTO UsersPasswordResetRequests (ID, UserId, CreatedOn) VALUES (@ID, @UserId, @CreatedOn)";
            await _dbConnection.ExecuteAsync(sql, new { Id = guid, UserId = user.Id, CreatedOn = DateTime.UtcNow });

            return guid;
        }
        public async Task ResetPassword(Guid passwordResetRequestId, string password)
        {
            // First remove all PasswordResetRequests older than 15' and then Retrieve the PasswordResetRequest
            string sql = @"DELETE FROM UsersPasswordResetRequests WHERE DATEADD(mi, 15, CreatedOn) < GETUTCDATE();
                    SELECT * FROM UsersPasswordResetRequests WHERE ID = @Id;";

            UsersPasswordResetRequest prr = await _dbConnection.QueryFirstOrDefaultAsync<UsersPasswordResetRequest>(sql, new { Id = passwordResetRequestId });

            if (prr == null)
                throw new Exception("Password change request has expired.");

            sql = "UPDATE Users SET [Password] = @Password WHERE ID = @Id; DELETE FROM UsersPasswordResetRequests WHERE ID = @Id";
            await _dbConnection.ExecuteAsync(sql, new { ID = prr.UserId, Password = password });
        }


        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertUsersAsync(Users user)
        {
            string sql = $"INSERT INTO Users(Created, CreatedBy, Updated, UpdatedBy, UserName, [Password], FirstName, LastName, CompanyId, EmailAddress, LastLogin, Address, AddressType, IsActive, RoleType, AgentType, AgentCompanyName, AgentCompanyAddress) VALUES(@Created, @CreatedBy, @Updated, @UpdatedBy, @UserName, @Password, @FirstName, @LastName, @CompanyId, @EmailAddress, @LastLogin, @Address, @AddressType, @IsActive, @RoleType, @AgentType, @AgentCompanyName, @AgentCompanyAddress)";
            return await _dbConnection.ExecuteAsync(sql, new { Created = user.Created, CreatedBy = user.CreatedBy, Updated = user.Updated, UpdatedBy = user.UpdatedBy, UserName = user.UserName, Password = user.Password, FirstName = user.FirstName, LastName = user.LastName, CompanyId = user.CompanyId, EmailAddress = user.EmailAddress, LastLogin = user.LastLogin, user.Address, AddressType= user.AddressType, IsActive = user.IsActive, RoleType = user.RoleType, user.AgentType, user.AgentCompanyName, user.AgentCompanyAddress });
        }

        public async Task<int> UpdateUsersAsync(Users user)
        {
            //IDbTransaction tr;
            //tr = _dbConnection.BeginTransaction();

            try
            {
                string updateSql = @"UPDATE Users SET Created = @Created, CreatedBy = @CreatedBy, 
             Updated = @Updated, UpdatedBy = @UpdatedBy, UserName = @UserName, 
             [Password] = @Password, FirstName = @FirstName, LastName = @LastName, 
             CompanyId = @CompanyId, EmailAddress = @EmailAddress, LastLogin = @LastLogin, 
             Address = @Address, AddressType = @AddressType, IsActive = @IsActive, RoleType = @RoleType, 
             AgentType = @AgentType, AgentCompanyName = @AgentCompanyName, 
             AgentCompanyAddress = @AgentCompanyAddress WHERE Id = @Id";

                await _dbConnection.ExecuteAsync(updateSql, new
                {
                    Created = user.Created,
                    CreatedBy = user.CreatedBy, // Provide the value for CreatedBy
                    Updated = user.Updated, // Provide the value for Updated
                    UpdatedBy = user.UpdatedBy, // Provide the value for UpdatedBy
                    UserName = user.UserName,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CompanyId = user.CompanyId,
                    EmailAddress = user.EmailAddress,
                    LastLogin = user.LastLogin,
                    Address = user.Address,
                    AddressType = user.AddressType,
                    IsActive = user.IsActive,
                    RoleType = user.RoleType,
                    AgentType = user.AgentType,
                    AgentCompanyName = user.AgentCompanyName,
                    AgentCompanyAddress = user.AgentCompanyAddress,
                    Id = user.Id
                });


                // Update or insert UserTelephones records
                string insertSql = @"INSERT INTO UserTelephones (TelephoneNumber, CreatedBy, Created, UserTelephoneId) 
                     VALUES (@TelephoneNumber, @CreatedBy, @Created, @UserTelephoneId)";

                string updateTelephoneSql = @"UPDATE UserTelephones SET TelephoneNumber = @TelephoneNumber, 
                              UpdatedBy = @UpdatedBy, Updated = @Updated WHERE UserTelephoneId = @UserTelephoneId";

                DateTime now = DateTime.UtcNow;
                await _dbConnection.ExecuteAsync("DELETE FROM UserTelephones WHERE UserTelephoneId NOT IN @Ids", new { Ids = user.UserTelephones.Where(sa => sa.UserTelephoneId != null).Select(sa => sa.UserTelephoneId) });

                //await _dbConnection.ExecuteAsync("DELETE FROM UserTelephones WHERE CreatedBy = @CreatedBy AND UserTelephoneId NOT IN @Ids", new { Ids = user.UserTelephones.Where(sa => sa.UserTelephoneId != null).Select(sa => sa.UserTelephoneId) });
                //await _dbConnection.ExecuteAsync("DELETE FROM UserTelephones WHERE UserTelephoneId=@UserTelephoneId");
                foreach (UserTelephone st in user.UserTelephones)
                {
                    
                    if (st.UserTelephoneId == null)
                    {
                        await _dbConnection.ExecuteAsync(insertSql, new { st.TelephoneNumber, CreatedBy = user.CreatedBy, Created = now, UserTelephoneId = Guid.NewGuid() });
                    }
                    else
                    {
                        await _dbConnection.ExecuteAsync(updateTelephoneSql, new { st.TelephoneNumber, UpdatedBy = st.UpdatedBy, Updated = now, UserTelephoneId = st.UserTelephoneId });
                    }
                }

                // Execute the update query for the main user details
                await _dbConnection.ExecuteAsync(updateSql, user);

            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return 1;
        }



        public async Task<int> UpdateUsersActive(Guid Id, bool IsActive)
        {
            string sql = $"UPDATE Users SET IsActive = @IsActive WHERE Id = @Id and IsActive = 1";
            return await _dbConnection.ExecuteAsync(sql, new { Id = Id, IsActive = IsActive });
        }
        public async Task<int> ActiveUsersAsync(Guid id, bool isActive, Guid updatedBy)
        {
            string sql = $"UPDATE Users SET IsActive = @IsActive, Updated = @Updated, UpdatedBy = @UpdatedBy WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id, IsActive = isActive, Updated = DateTime.UtcNow, UpdatedBy = updatedBy });
        }

        public async Task<int> UpdateUsersRoleTypeAsync(Guid id, byte? roleType, Guid updatedBy)
        {
            string sql = $"UPDATE Users SET RoleType = @RoleType, Updated = @Updated, UpdatedBy = @UpdatedBy WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id, RoleType = roleType, Updated = DateTime.UtcNow, UpdatedBy = updatedBy });
        }

        public async Task<int> UpdateUsersPasswordAsync(Guid id, string password, Guid updatedBy)
        {
            string sql = $"UPDATE Users SET Password = @Password, Updated = @Updated, UpdatedBy = @UpdatedBy WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id, Password = password, Updated = DateTime.UtcNow, UpdatedBy = updatedBy });
        }

        public async Task<int> UpdateUsersLastLoginAsync(Guid id)
        {
            string sql = $"UPDATE Users SET LastLogin = @LastLogin WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id, LastLogin = DateTime.UtcNow });
        }
        #endregion
    }
}
