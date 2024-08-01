using CaseManagementSystem.Emails.Templates;
using CMS.DL.DM;
using CMS.DL.Model;

namespace CaseManagementSystem.Data.Cases
{
    public class CaseWatchersService
    {
        private readonly string _connectionString = "";

        public CaseWatchersService(IConfiguration configuration) 
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        #region GET
        public async Task<CaseWatchers> GetCaseWatcherAsync(Guid caseId, Guid userId)
        {
            CaseWatchersDM caseWatchersDM = new CaseWatchersDM(_connectionString);
            CaseWatchers cw = await caseWatchersDM.GetCaseWatcherAsync(caseId, userId);
            return cw;
        }
        public async Task<List<CaseWatchers>> GetCaseWatchersByCaseIdAsync(Guid caseId)
        {
            CaseWatchersDM caseWatchersDM = new CaseWatchersDM(_connectionString);
            return await caseWatchersDM.GetCaseWatchersByCaseIdAsync(caseId);
        }
        
        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertCasesAsync(CaseWatchers caseWatcher)
        {
            CaseWatchersDM casesDM = new CaseWatchersDM(_connectionString);
            int result = await casesDM.InsertCasesAsync(caseWatcher);
            return result;
        }

        public async Task<int> DeleteCasesAsync(Guid caseId, Guid userId)
        {
            CaseWatchersDM casesDM = new CaseWatchersDM(_connectionString);
            int result = await casesDM.DeleteCasesAsync(caseId, userId);
            return result;

            return result;
        }

        #endregion
    }
}
