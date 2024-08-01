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
    public class CasesDM
    {
        private readonly IDbConnection _dbConnection;

        public CasesDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        #region GET

        public async Task<IEnumerable<Cases>> GetAllCasesAsync()
        {
            string sql = $@"SELECT C.*,crd.FilePath, CO.CompanyName,CU.Address as ClientAddress, CU.EmailAddress as ClientEmail,
                            CU.TelphoneNumber as ClientTelephoneNo, TL.Name as TraceLevelName, TR.Name as TraceReasonName,
                            CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName,
                            CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName FROM Cases AS C 
                            LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id 
                            LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id LEFT JOIN Subjects AS S ON C.SubjectId = S.Id 
                            LEFT JOIN Users AS CU ON C.ClientRef = CU.Id LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
                            Left join CaseReportDocuments crd  on crd.CaseId = C.Id
                            ORDER BY CaseNumber DESC";
            return await _dbConnection.QueryAsync<Cases>(sql);
        }

        public async Task<Cases> GetCasesByIdAsync(Guid id)
        {
            string sql = $"SELECT C.*, CO.CompanyName,CU.EmailAddress, TL.Name as TraceLevelName, TR.Name as TraceReasonName, CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName FROM Cases AS C LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id LEFT JOIN Subjects AS S ON C.SubjectId = S.Id LEFT JOIN Users AS CU ON C.ClientRef = CU.Id LEFT JOIN Users AS AU ON C.EndClient = AU.Id WHERE C.Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Cases>(sql, new { Id = id });
        }

        //public async Task<Cases> GetCasesByCreatedBy(Guid? CreatedBy)
        //{
        //    string sql = $"SELECT C.*, CO.CompanyName,CU.EmailAddress, TL.Name as TraceLevelName, TR.Name as TraceReasonName, CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName FROM Cases AS C LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id LEFT JOIN Subjects AS S ON C.SubjectId = S.Id LEFT JOIN Users AS CU ON C.ClientRef = CU.Id LEFT JOIN Users AS AU ON C.EndClient = AU.Id WHERE C.Id = @Id";
        //    return await _dbConnection.QueryFirstOrDefaultAsync<Cases>(sql, new { Id = CreatedBy });
        //}


        public async Task<IEnumerable<Cases>> GetCasesByCreatedBy(Guid? CreatedBy)
        {
            string sql = $"SELECT C.*, CO.CompanyName, CU.EmailAddress, TL.Name as TraceLevelName, TR.Name as TraceReasonName, CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName FROM Cases AS C LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id LEFT JOIN Subjects AS S ON C.SubjectId = S.Id LEFT JOIN Users AS CU ON C.ClientRef = CU.Id LEFT JOIN Users AS AU ON C.EndClient = AU.Id WHERE C.CreatedBy = @CreatedBy";
            return await _dbConnection.QueryAsync<Cases>(sql, new { CreatedBy });
        }

        public async Task<Cases> GetLastCaseAsync(string type)
        {
            string sql = "";
            if (type == "Subject")
            {
                 sql = $"SELECT TOP 1 C.*, CO.CompanyName, TL.Name as TraceLevelName, TR.Name as TraceReasonName, CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName " +
               $"FROM Cases AS C " +
               $"LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id " +
               $"LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id " +
               $"LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id " +
               $"LEFT JOIN Subjects AS S ON C.SubjectId = S.Id " +
               $"LEFT JOIN Users AS CU ON C.ClientRef = CU.Id " +
               $"LEFT JOIN Users AS AU ON C.EndClient = AU.Id " +
               $"ORDER BY S.Created DESC";
             
            }
            else
            {
                 sql = $"SELECT TOP 1 C.*, CO.CompanyName, TL.Name as TraceLevelName, TR.Name as TraceReasonName, CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName " +
          $"FROM Cases AS C " +
          $"LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id " +
          $"LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id " +
          $"LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id " +
          $"LEFT JOIN Subjects AS S ON C.SubjectId = S.Id " +
          $"LEFT JOIN Users AS CU ON C.ClientRef = CU.Id " +
          $"LEFT JOIN Users AS AU ON C.EndClient = AU.Id " +
          $"ORDER BY C.Created DESC";
            }
      

            return await _dbConnection.QueryFirstOrDefaultAsync<Cases>(sql);
        }

        public async Task<IEnumerable<Cases>> GetCasesByClientAsync(Guid? id)
        {
            string sql = $@"SELECT 
                    C.*,crd.FilePath ,CO.CompanyName, 
                    TL.Name as TraceLevelName, 
                    TR.Name as TraceReasonName, 
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, 
                    CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
                    CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName 
                FROM Cases AS C 
                    LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id 
                    LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id 
                    LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id 
                    LEFT JOIN Subjects AS S ON C.SubjectId = S.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
                    LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
					Left join CaseReportDocuments crd  on crd.CaseId = C.Id
                WHERE C.CreatedBy = @CreatedBy OR C.ClientRef = @ClientRef";
            return await _dbConnection.QueryAsync<Cases>(sql, new { CreatedBy = id, ClientRef = id });
        }

        public async Task<IEnumerable<Cases>> GetCasesByAgentAsync(Guid? id)
        {
            string sql = $@"SELECT 
                    C.*, CO.CompanyName, 
                    TL.Name as TraceLevelName, 
                    TR.Name as TraceReasonName, 
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, 
                    CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
                    CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName 
                FROM Cases AS C 
                    LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id 
                    LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id 
                    LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id 
                    LEFT JOIN Subjects AS S ON C.SubjectId = S.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
                    LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
                WHERE C.CreatedBy = @CreatedBy OR C.EndClient = @EndClient OR C.EndClient IS NULL OR C.Status IN (1,5,6,7)";
            return await _dbConnection.QueryAsync<Cases>(sql, new { CreatedBy = id, EndClient = id });
        }

        public async Task<List<int>> GetCaseNumbers(Guid? CompanyId, Guid? SubjectId)
        {
            string sql = "SELECT CaseNumber FROM Cases WHERE SubjectId = @SubjectId AND CompanyId = @CompanyId";
            var caseNumbers = await _dbConnection.QueryAsync<int>(sql, new { CompanyId, SubjectId });
            return caseNumbers.ToList();
        }
        public async Task<Guid> GetCaseIdByCaseNumber(int caseNumber)
        {
            string sql = "SELECT Id FROM Cases WHERE CaseNumber = @caseNumber";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(sql, new { CaseNumber = caseNumber });
            return result;
        }

        public async Task<List<Cases>> DateFilter(DateTime start, DateTime end)
        {
            string sql = $@"SELECT C.*, CO.CompanyName, 
                    TL.Name as TraceLevelName, 
                    TR.Name as TraceReasonName, 
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, 
                    CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
                    CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName 
                FROM Cases AS C 
                    LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id 
                    LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id 
                    LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id 
                    LEFT JOIN Subjects AS S ON C.SubjectId = S.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
                    LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
                  WHERE C.Created >= @Start  AND C.Created <= @End";

            var result = await _dbConnection.QueryAsync<Cases>(sql, new { Start = start, End = end });
                return result.ToList();
        }
        public async Task<Guid> GetCaseWatchApproved(Guid? id)
        {
            string sql = $"SELECT CASE WHEN CaseId IS NULL THEN 'null' ELSE CaseId END AS CaseId FROM CaseWatchers where CaseId = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Guid>(sql, new { Id = id });
        }
        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertCasesAsync(Cases cases)
        {
            string sql = $"INSERT INTO Cases(Created, CreatedBy,CreatedByName, Updated, UpdatedBy, CompanyId, IndividualId, ClientReference, TraceLevelId, Fee, DebtAmount, ClientRef, EndClient, TraceReasonId, Notes, Status, SubjectId, CaseLinkId, EndClientFreeText) VALUES(@Created, @CreatedBy, @CreatedByName, @Updated, @UpdatedBy, @CompanyId, @IndividualId, @ClientReference, @TraceLevelId, @Fee, @DebtAmount, @ClientRef, @EndClient, @TraceReasonId, @Notes, @Status, @SubjectId, @CaseLinkId, @EndClientFreeText)";
            return await _dbConnection.ExecuteAsync(sql, new { Created = cases.Created, CreatedBy = cases.CreatedBy, CreatedByName=cases.CreatedByName, Updated = cases.Updated, UpdatedBy = cases.UpdatedBy, CompanyId = cases.CompanyId, IndividualId = cases.IndividualId, ClientReference = cases.ClientReference, TraceLevelId = cases.TraceLevelId, Fee = cases.Fee, DebtAmount = cases.DebtAmount, ClientRef = cases.ClientRef, EndClient = cases.EndClient, TraceReasonId = cases.TraceReasonId, Notes = cases.Notes, Status = cases.Status, SubjectId = cases.SubjectId, CaseLinkId = cases.CaseLinkId, EndClientFreeText = cases.EndClientFreeText });
        }

        public async Task<int> UpdateCasesAsync(Cases cases)
        {
            string sql = $"UPDATE Cases SET Created = @Created, CreatedBy = @CreatedBy, CreatedByName = @CreatedByName,  Updated = @Updated, UpdatedBy = @UpdatedBy, CompanyId = @CompanyId, IndividualId = @IndividualId, ClientReference = @ClientReference, TraceLevelId = @TraceLevelId, Fee = @Fee, DebtAmount = @DebtAmount, ClientRef = @ClientRef, EndClient = @EndClient, TraceReasonId = @TraceReasonId, Notes = @Notes, Status = @Status, SubjectId = @SubjectId, CaseLinkId = @CaseLinkId, EndClientFreeText=@EndClientFreeText, IsSynced=@IsSynced WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Created = cases.Created, CreatedBy = cases.CreatedBy, CreatedByName = cases.CreatedByName, Updated = cases.Updated, UpdatedBy = cases.UpdatedBy, CompanyId = cases.CompanyId, IndividualId = cases.IndividualId, ClientReference = cases.ClientReference, TraceLevelId = cases.TraceLevelId, Fee = cases.Fee, DebtAmount = cases.DebtAmount, ClientRef = cases.ClientRef, EndClient = cases.EndClient, TraceReasonId = cases.TraceReasonId, Notes = cases.Notes, Status = cases.Status, SubjectId = cases.SubjectId, CaseLinkId = cases.CaseLinkId, EndClientFreeText=cases.EndClientFreeText, IsSynced = cases.IsSynced, Id = cases.Id });
        }
        public async Task<int> UpdateCaseTraceQuestions(Guid caseId, List<CaseTraceQuestions> traceQuestions)
        {
            string sql = $"DELETE FROM CaseTraceQuestions WHERE CaseId = @caseId";
            await _dbConnection.ExecuteAsync(sql, new { caseId });

            sql = $"INSERT INTO CaseTraceQuestions (ID, CaseId, Question, Answer, Created, CreatedBy) VALUES (@ID, @CaseId, @Question, @Answer, @Created, @CreatedBy)";
            foreach (CaseTraceQuestions tq in traceQuestions) {
                await _dbConnection.ExecuteAsync(sql, new { ID = tq.Id, CaseId = caseId, Question = tq.Question, Answer = tq.Answer, Created = tq.Created, CreatedBy = tq.CreatedBy});
            }
            return 1;
        }

        public async Task<int> DeleteCasesAsync(Guid id)
        {
            string sql = $"DELETE FROM Cases WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<int> GetApprovedCasesCount(Guid? id)
        {
            string sql = $"select COUNT(Id)ApprovedCases from Cases where EndClient = @Id and Status  = 2";
            var Count = await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, new { Id = id });
            return Count;
        }

        public async Task<int> SaveCaseReportFile(Guid CaseReportId, Guid? CaseId, Guid? SubjectId, string FilePath)
        {
            string sql = $@"Insert into CaseReportDocuments(Id, CaseId, SubjectId, FilePath) Values(@CaseReportId,@CaseId, @SubjectId, @FilePath)";
            return await _dbConnection.ExecuteAsync(sql, new { CaseReportId = CaseReportId , CaseId = CaseId , SubjectId = SubjectId , FilePath = FilePath });
        }
        public async Task<Cases> GetCaseReportFile(Guid? CaseId, Guid? SubjectId)
        {
            string sql = $"select * from CaseReportDocuments where CaseId= @CaseId and SubjectId = @SubjectId";
            var value = await _dbConnection.QueryFirstOrDefaultAsync<Cases>(sql, new { CaseId = CaseId, SubjectId = SubjectId});
            return value;
        }

        public async Task<Guid> DeleteCaseReportFile(Guid SubjectId)
        {
            string sql = "DELETE FROM CaseReportDocuments WHERE Id = @SubjectId";
            await _dbConnection.ExecuteAsync(sql, new { SubjectId });
            return SubjectId;
        }
    }
    #endregion
}

