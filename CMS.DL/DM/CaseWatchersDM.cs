using CMS.DL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;
using System.Data;
using static Azure.Core.HttpHeader;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace CMS.DL.DM
{
    public class CaseWatchersDM
    {
        private readonly IDbConnection _dbConnection;

        public CaseWatchersDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<CaseWatchers> GetCaseWatcherAsync(Guid caseId, Guid userId)
        {
            string sql = $"SELECT * FROM CaseWatchers WHERE UserId = @UserId AND CaseId=@CaseId";
            return (await _dbConnection.QueryAsync<CaseWatchers>(sql, new { CaseId = caseId, UserId = userId })).FirstOrDefault();
        }

        public async Task<List<CaseWatchers>> GetCaseWatchersByCaseIdAsync(Guid caseId)
        {
            string sql = $"SELECT CW.*, U.FirstName, U.EmailAddress AS WatcherEmailAddress FROM CaseWatchers CW " +
                $"INNER JOIN Users U ON U.Id = CW.UserId WHERE CaseId=@CaseId";
            return (await _dbConnection.QueryAsync<CaseWatchers>(sql, new { CaseId = caseId})).ToList();
        }

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertCasesAsync(CaseWatchers caseWatcher)
        {
            string sql = $"INSERT INTO CaseWatchers(CaseId, UserId, Created) VALUES(@CaseId, @UserId, @Created)";
            return await _dbConnection.ExecuteAsync(sql, new { Created = caseWatcher.Created, CaseId = caseWatcher.CaseId, UserId = caseWatcher.UserId});
        }

        public async Task<int> DeleteCasesAsync(Guid caseId, Guid userId)
        {
            string sql = $"DELETE FROM CaseWatchers WHERE UserId = @UserId AND CaseId=@CaseId";
            return await _dbConnection.ExecuteAsync(sql, new { CaseId = caseId, UserId = userId });
        }

        #endregion
    }
}
