using CMS.DL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CMS.DL.DM
{
    public class SubjectsDM
    {
        private readonly IDbConnection _dbConnection;

        public SubjectsDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        #region GET

        public async Task<IEnumerable<Subjects>> GetAllSubjectsAsync()
        {
            string sql = "SELECT * FROM Subjects";
            return await _dbConnection.QueryAsync<Subjects>(sql);
        }

        public async Task<IEnumerable<Subjects>> GetAllSubjectsWithCaseAsync()
        {
//            string sql = "SELECT DISTINCT S.*,TP.[Name] AS TitlePrfixName,C.CaseNumber,C.ClientRef,C.EndClient,CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName, CO.CompanyName FROM Subjects AS S LEFT JOIN Cases AS C ON S.Id = C.SubjectId LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id LEFT JOIN Users AS CU ON C.ClientRef = CU.Id LEFT JOIN Users AS AU ON C.EndClient = AU.Id LEFT JOIN Companies AS CO ON S.AssociatedCompany = CO.Id";
            string sql = @"SELECT 
                    S.*,TP.[Name] AS TitlePrfixName,
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName
                FROM Subjects AS S 
                    LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id";
            return await _dbConnection.QueryAsync<Subjects>(sql);
        }

        public async Task<IEnumerable<Subjects>> GetSubjectsByClientAsync(Guid? id)
        {
//            string sql = $"SELECT DISTINCT S.*,TP.[Name] AS TitlePrfixName,C.CaseNumber,C.ClientRef,C.EndClient,CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName, CO.CompanyName FROM Subjects AS S LEFT JOIN Cases AS C ON S.Id = C.SubjectId LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id LEFT JOIN Users AS CU ON C.ClientRef = CU.Id LEFT JOIN Users AS AU ON C.EndClient = AU.Id LEFT JOIN Companies AS CO ON S.AssociatedCompany = CO.Id WHERE S.CreatedBy = @CreatedBy";
            string sql = $@"SELECT 
                    S.*,
                    TP.[Name] AS TitlePrfixName,
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName
                FROM Subjects AS S 
                    LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
                WHERE S.CreatedBy = @CreatedBy";
            return await _dbConnection.QueryAsync<Subjects>(sql, new { CreatedBy = id });
        }

        public async Task<Subjects> GetSubjectsByIdAsync(Guid id)
        {
            string sql = @"SELECT 
                    S.*,TP.[Name] AS TitlePrfixName,C.CaseNumber,C.ClientRef,C.EndClient,CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
                    CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName 
                FROM Subjects AS S 
                    LEFT JOIN Cases AS C ON S.Id = C.SubjectId 
                    LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
                WHERE S.Id = @Id";
            Subjects subject = await _dbConnection.QueryFirstOrDefaultAsync<Subjects>(sql, new { Id = id });

            subject.SubjectAddresses = (await _dbConnection.QueryAsync<SubjectAddresses>("SELECT * FROM SubjectAddresses WHERE SubjectId = @Id", new { Id = id })).ToList();
            subject.SubjectEmails = (await _dbConnection.QueryAsync<SubjectEmail>("SELECT * FROM SubjectEmail WHERE SubjectId = @Id", new { Id = id })).ToList();
            subject.SubjectCompanies = (await _dbConnection.QueryAsync<SubjectCompany>("SELECT * FROM SubjectCompany WHERE SubjectId = @Id", new { Id = id })).ToList();
            subject.SubjectAliases = (await _dbConnection.QueryAsync<SubjectAlias>("SELECT * FROM SubjectAlias WHERE SubjectId = @Id", new { Id = id })).ToList();
            subject.SubjectTelephones = (await _dbConnection.QueryAsync<SubjectTelephone>("SELECT * FROM SubjectTelephones WHERE SubjectId = @Id", new { Id = id })).ToList();
            return subject;
        }

        public async Task<Subjects> GetLastSubjectsAsync()
        {
            string sql = $@"SELECT 
                    TOP 1 S.*,TP.[Name] AS TitlePrfixName,C.CaseNumber,C.ClientRef,C.EndClient,
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
                    CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName 
                FROM Subjects AS S 
                    LEFT JOIN Cases AS C ON S.Id = C.SubjectId 
                    LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
                    LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
                ORDER BY S.Created DESC";
            return await _dbConnection.QueryFirstOrDefaultAsync<Subjects>(sql);
        }

        public async Task<IEnumerable<Subjects>> SearchSubjectsAsync(
    string firstName,
    string lastName,
    string alias,
    string address,
    string dob)
        {
            if ((firstName ?? string.Empty).Trim() != string.Empty)
                firstName = $"%{firstName}%";

            if ((lastName ?? string.Empty).Trim() != string.Empty)
                lastName = $"%{lastName}%";
            if ((alias ?? string.Empty).Trim() != string.Empty)
                alias = $"%{alias}%";
            if ((address ?? string.Empty).Trim() != string.Empty)
                address = $"%{address}%";

            string sql = $@"SELECT 
        S.*,TP.[Name] AS TitlePrfixName,CONCAT(S.FirstName, ' ', S.LastName) as SubjectName
    FROM Subjects AS S 
        LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
    WHERE (S.FirstName LIKE @FirstName OR @FirstName IS NULL)
          OR (S.LastName LIKE @LastName OR @LastName IS NULL)
          OR (S.Alias LIKE @Alias OR @Alias IS NULL)
          OR (S.Addresses LIKE @Addresses OR @Addresses IS NULL)
          OR (S.DateOfBirth = @DateOfBirth OR @DateOfBirth IS NULL)";
            return await _dbConnection.QueryAsync<Subjects>(sql, new { FirstName = firstName, LastName = lastName, Alias = alias, Addresses = address, DateOfBirth = dob });
        }


        //public async Task<IEnumerable<Subjects>> GetAllSubjectsWithCaseByAgentAsync(Guid id)
        //{
        //    string sql = @"SELECT 
        //            S.*,TP.[Name] AS TitlePrfixName,C.CaseNumber,C.ClientRef,C.EndClient,
        //            CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, 
        //            CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
        //            CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName, 
        //            C.Created AS CaseCreated, C.Id AS CaseId, C.Notes AS CaseNotes, C.Status AS CaseStatus 
        //        FROM Cases AS C 
        //            INNER JOIN Subjects AS S ON C.SubjectId = S.Id 
        //            LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
        //            LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
        //            LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
        //        WHERE C.EndClient = @EndClient OR C.EndClient IS NULL OR C.Status IN (1,5,6,7)";
        //    return await _dbConnection.QueryAsync<Subjects>(sql, new { EndClient = id });
        //}

        public async Task<IEnumerable<Subjects>> GetAllSubjectsWithCaseByAgentAsync(Guid id)
        {
            string sql = @"SELECT 
           S.*, TP.[Name] AS TitlePrfixName, C.CaseNumber, C.ClientRef, C.EndClient,
           CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, 
           CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
           CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName, 
           C.Created AS CaseCreated, C.Id AS CaseId, C.Notes AS CaseNotes, C.Status AS CaseStatus 
       FROM Cases AS C 
           INNER JOIN Subjects AS S ON C.SubjectId = S.Id 
           LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
           LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
           LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
       WHERE C.EndClient = @EndClient";
            return await _dbConnection.QueryAsync<Subjects>(sql, new { EndClient = id });
        }



        public async Task<IEnumerable<Subjects>> GetAllSubjectsWithCaseAsync(Guid id)
        {
            string sql = @"SELECT 
                    S.*,TP.[Name] AS TitlePrfixName,C.CaseNumber,C.ClientRef,C.EndClient,
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, 
                    CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
                    CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName, 
                    C.Created AS CaseCreated, C.Id AS CaseId, C.Notes AS CaseNotes, C.Status AS CaseStatus 
                FROM Cases AS C 
                    INNER JOIN Subjects AS S ON C.SubjectId = S.Id 
                    LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
                    LEFT JOIN Users AS AU ON C.EndClient = AU.Id";
            return await _dbConnection.QueryAsync<Subjects>(sql, new { EndClient = id });
        }

        public async Task<Subjects> GetSubjectsWithCaseByCaseIdAsync(Guid id)
        {
            string sql = @"SELECT 
                    S.*,TP.[Name] AS TitlePrfixName,C.CaseNumber,C.ClientRef,C.EndClient,
                    CONCAT(S.FirstName, ' ', S.LastName) as SubjectName, CONCAT(CU.FirstName, ' ', CU.LastName) AS ClientName, 
                    CONCAT(AU.FirstName, ' ', AU.LastName) AS AgentName, C.Created AS CaseCreated, C.Id AS CaseId, C.Notes AS CaseNotes, C.Status AS CaseStatus
                FROM Cases AS C 
                    INNER JOIN Subjects AS S ON C.SubjectId = S.Id 
                    LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
                    LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
                WHERE C.Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Subjects>(sql, new { Id = id });
        }

        public async Task<IEnumerable<SubjectChanges>> GetChangesDoneOnACaseAsync(Guid caseId)
        {
            string sql = "EXEC GetSubjectCaseChanges @caseId";
            return await _dbConnection.QueryAsync<SubjectChanges>(sql, new { CaseId = caseId });
        }

        public async Task<IEnumerable<SubjectCaseTraceQuestions>> GetTraceQuestionsAnswersAsync(Guid caseId)
        {
            string sql = "EXEC GetSubjectCaseQandA @caseId";
            return await _dbConnection.QueryAsync<SubjectCaseTraceQuestions>(sql, new { CaseId = caseId });
        }
        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<Guid> InsertSubjectsAsync(Subjects subjects)
        {
            IDbTransaction tr;
            Guid subjectId;

            _dbConnection.Open();
            tr = _dbConnection.BeginTransaction();
            try
            {
                string sql = $@"INSERT INTO Subjects
                (Created, CreatedBy, Updated, UpdatedBy, FirstName, MiddleName, LastName, DateOfBirth, Alias, Notes, Email,Company, TelephoneNumber, Addresses, TitlePrefixId) 
                VALUES
                (@Created, @CreatedBy, @Updated, @UpdatedBy, @FirstName, @MiddleName, @LastName, @DateOfBirth, @Alias, @Notes, @Email,@Company, @TelephoneNumber, @Addresses, @TitlePrefixId); SELECT TOP 1 Id FROM Subjects ORDER BY Created DESC";


                subjectId = await _dbConnection.ExecuteScalarAsync<Guid>(sql, new
                {
                    Created = subjects.Created,
                    CreatedBy = subjects.CreatedBy,
                    Updated = subjects.Updated,
                    UpdatedBy = subjects.UpdatedBy,
                    FirstName = subjects.FirstName,
                    MiddleName = subjects.MiddleName,
                    LastName = subjects.LastName,
                    DateOfBirth = subjects.DateOfBirth,
                    Alias = subjects.Alias,
                    Notes = subjects.Notes,
                    Email = subjects.Email,
                    Company=subjects.Company,
                    TelephoneNumber = subjects.TelephoneNumber,
                    Addresses = subjects.Addresses,
                    TitlePrefixId = subjects.TitlePrefixId
                },tr);

                foreach(SubjectAddresses sa in subjects.SubjectAddresses)
                {
                    sql = $"INSERT INTO SubjectAddresses (SubjectId, Address, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @Address, @CaseId, @CreatedBy, @Created, @Approved)";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, sa.Address, sa.CaseId, sa.Created, sa.CreatedBy, sa.Approved }, tr);
                }

                foreach (SubjectAlias sa in subjects.SubjectAliases)
                {
                    sql = $"INSERT INTO SubjectAlias (SubjectId, Alias, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @Alias, @CaseId, @CreatedBy, @Created, @Approved)";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, sa.Alias, sa.CaseId, sa.Created, sa.CreatedBy, sa.Approved }, tr);
                }

                foreach (SubjectEmail se in subjects.SubjectEmails)
                {
                    sql = $"INSERT INTO SubjectEMail (SubjectId, EMail, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @EMail, @CaseId, @CreatedBy, @Created, @Approved)";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, se.Email, se.CaseId, se.Created, se.CreatedBy, se.Approved }, tr);
                }
                foreach (SubjectCompany se in subjects.SubjectCompanies)
                {
                    Guid SubjectCompanyId = Guid.NewGuid();
                    sql = $"INSERT INTO SubjectCompany (SubjectCompanyId,SubjectId, Company, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectCompanyId,@SubjectId, @Company, @CaseId, @CreatedBy, @Created, @Approved)";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectCompanyId= SubjectCompanyId, SubjectId = subjectId, se.Company, se.CaseId, se.Created, se.CreatedBy, se.Approved }, tr);
                }

                foreach (SubjectTelephone st in subjects.SubjectTelephones)
                {
                    sql = $"INSERT INTO SubjectTelephones (SubjectId, TelephoneNumber, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @TelephoneNumber, @CaseId, @CreatedBy, @Created, @Approved)";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, st.TelephoneNumber, st.CaseId, st.Created, st.CreatedBy, st.SubjectTelephoneId, st.Approved }, tr);
                }


                tr.Commit();

                _dbConnection.Close();
            }
            catch (Exception ex) {
                tr.Rollback();
                _dbConnection.Close();
                throw ex;
            }

            return subjectId;
        }

        public async Task<int> UpdateSubjectsAsync(Subjects subjects)
        {
            IDbTransaction tr;
            Guid subjectId = subjects.Id;
            _dbConnection.Open();
            tr = _dbConnection.BeginTransaction();
            try
            {
                string sql = $@"UPDATE Subjects SET Created = @Created, CreatedBy = @CreatedBy, Updated = @Updated, UpdatedBy = @UpdatedBy, FirstName = @FirstName, 
                MiddleName = @MiddleName, LastName = @LastName, DateOfBirth = @DateOfBirth, Alias = @Alias, Notes = @Notes , Email = @Email,Company=@Company, TelephoneNumber = @TelephoneNumber, 
                Addresses = @Addresses, TitlePrefixId = @TitlePrefixId WHERE Id = @Id ";
                
                await _dbConnection.ExecuteAsync(sql, new { Created = subjects.Created, CreatedBy = subjects.CreatedBy, Updated = subjects.Updated, UpdatedBy = subjects.UpdatedBy, FirstName = subjects.FirstName, 
                MiddleName = subjects.MiddleName, LastName = subjects.LastName, DateOfBirth = subjects.DateOfBirth, Alias = subjects.Alias, Notes = subjects.Notes, Email = subjects.Email,Company=subjects.Company, 
                TelephoneNumber = subjects.TelephoneNumber, Addresses = subjects.Addresses, TitlePrefixId = subjects.TitlePrefixId, Id = subjects.Id }, tr);

                await _dbConnection.ExecuteAsync("DELETE FROM SubjectAddresses WHERE SubjectId = @SubjectId AND SubjectAddressId NOT IN @Ids", new { SubjectId = subjectId, Ids = subjects.SubjectAddresses.Where(sa => sa.SubjectAddressId != null).Select(sa => sa.SubjectAddressId) }, tr);
                foreach (SubjectAddresses sa in subjects.SubjectAddresses)
                {
                    if (sa.SubjectAddressId == null)
                        sql = $"INSERT INTO SubjectAddresses (SubjectId, Address, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @Address, @CaseId, @CreatedBy, @Created, @Approved)";
                    else
                        sql = $"UPDATE SubjectAddresses SET SubjectId = @SubjectId, Address = @Address, CaseId = @CaseId, CreatedBy = @CreatedBy, Created = @Created, Approved = @Approved WHERE SubjectAddressId = @SubjectAddressId";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, sa.Address, sa.CaseId, sa.Created, sa.CreatedBy, sa.SubjectAddressId, sa.Approved }, tr);
                }

                await _dbConnection.ExecuteAsync("DELETE FROM SubjectAlias WHERE SubjectId = @SubjectId AND SubjectAliasId NOT IN @Ids", new { SubjectId = subjectId, Ids = subjects.SubjectAliases.Where(sa => sa.SubjectAliasId != null).Select(sa => sa.SubjectAliasId) }, tr);
                foreach (SubjectAlias sa in subjects.SubjectAliases)
                {
                    if (sa.SubjectAliasId == null)
                        sql = $"INSERT INTO SubjectAlias (SubjectId, Alias, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @Alias, @CaseId, @CreatedBy, @Created, @Approved)";
                    else
                        sql = $"UPDATE SubjectAlias SET SubjectId = @SubjectId, Alias = @Alias, CaseId = @CaseId, CreatedBy = @CreatedBy, Created = @Created, Approved = @Approved WHERE SubjectAliasId = @SubjectAliasId";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, sa.Alias, sa.CaseId, sa.Created, sa.CreatedBy, sa.SubjectAliasId, sa.Approved }, tr);
                }

                await _dbConnection.ExecuteAsync("DELETE FROM SubjectEMail WHERE SubjectId = @SubjectId AND SubjectEmailId NOT IN @Ids", new { SubjectId = subjectId, Ids = subjects.SubjectEmails.Where(sa => sa.SubjectEmailId != null).Select(sa => sa.SubjectEmailId) }, tr);
                foreach (SubjectEmail se in subjects.SubjectEmails)
                {
                    if (se.SubjectEmailId == null)
                        sql = $"INSERT INTO SubjectEMail (SubjectId, EMail, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @EMail, @CaseId, @CreatedBy, @Created, @Approved)";
                    else
                        sql = $"UPDATE SubjectEMail SET SubjectId = @SubjectId, EMail = @EMail, CaseId = @CaseId, CreatedBy = @CreatedBy, Created = @Created, Approved = @Approved WHERE SubjectEmailId = @SubjectEmailId";

                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, se.Email, se.CaseId, se.Created, se.CreatedBy, se.SubjectEmailId, se.Approved }, tr);
                }

                await _dbConnection.ExecuteAsync("DELETE FROM SubjectCompany WHERE SubjectId = @SubjectId AND SubjectCompanyId NOT IN @Ids", new { SubjectId = subjectId, Ids = subjects.SubjectCompanies.Where(sa => sa.SubjectCompanyId != null).Select(sa => sa.SubjectCompanyId) }, tr);
                foreach (SubjectCompany se in subjects.SubjectCompanies)
                {
                    Guid SubjectCompanyId = Guid.NewGuid();
                    if (se.SubjectCompanyId == null)
                        sql = $"INSERT INTO SubjectCompany (SubjectCompanyId,SubjectId, Company, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectCompanyId,@SubjectId, @Company, @CaseId, @CreatedBy, @Created, @Approved)";
                    else
                        sql = $"UPDATE SubjectCompany SET SubjectCompanyId=@SubjectCompanyId, SubjectId = @SubjectId, Company = @Company, CaseId = @CaseId, CreatedBy = @CreatedBy, Created = @Created, Approved = @Approved WHERE SubjectCompanyId = @SubjectCompanyId";

                    await _dbConnection.ExecuteAsync(sql, new { SubjectCompanyId= SubjectCompanyId, SubjectId = subjectId, se.Company, se.CaseId, se.Created, se.CreatedBy, se.Approved }, tr);
                }


                await _dbConnection.ExecuteAsync("DELETE FROM SubjectTelephones WHERE SubjectId = @SubjectId AND SubjectTelephoneId NOT IN @Ids", new { SubjectId = subjectId, Ids = subjects.SubjectTelephones.Where(sa => sa.SubjectTelephoneId != null).Select(sa => sa.SubjectTelephoneId) }, tr);
                foreach (SubjectTelephone st in subjects.SubjectTelephones)
                {
                    if (st.SubjectTelephoneId == null)
                        sql = $"INSERT INTO SubjectTelephones (SubjectId, TelephoneNumber, CaseId, CreatedBy, Created, Approved) VALUES (@SubjectId, @TelephoneNumber, @CaseId, @CreatedBy, @Created, @Approved)";
                    else
                        sql = $"UPDATE SubjectTelephones SET SubjectId = @SubjectId, TelephoneNumber = @TelephoneNumber, CaseId = @CaseId, CreatedBy = @CreatedBy, Created = @Created, Approved = @Approved WHERE SubjectTelephoneId = @SubjectTelephoneId";
                    await _dbConnection.ExecuteAsync(sql, new { SubjectId = subjectId, st.TelephoneNumber, st.CaseId, st.Created, st.CreatedBy, st.SubjectTelephoneId, st.Approved }, tr);
                }

                tr.Commit();
                _dbConnection.Close();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                _dbConnection.Close();
                throw ex;
            }
            return 1;
        }

        public async Task<int> DeleteSubjectsAsync(Guid id)
        {
            string sql = $"DELETE FROM Subjects WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        #endregion
    }
}
