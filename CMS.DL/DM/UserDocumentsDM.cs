using CMS.DL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.DM
{
    public class UserDocumentsDM
    {
        private readonly IDbConnection _dbConnection;

        public UserDocumentsDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        #region GET

        public async Task<IEnumerable<UserDocuments>> GetAllUserDocumentsAsync()
        {
            string sql = "SELECT * FROM UserDocuments";
            return await _dbConnection.QueryAsync<UserDocuments>(sql);
        }
        //public async Task<List<UserDocuments>> GetUserDocumentsByIdAsync(Guid id)
        //{
        //    string sql = "SELECT * FROM UserDocuments WHERE CreatedBy = @id";
        //    return await _dbConnection.QueryFirstOrDefault<UserDocuments>(sql, new { CreatedBy = id });
        //}
        public async Task<IEnumerable<UserDocuments>>GetUserDocumentsById(Guid createdBy)
        {           
                string sql = $"SELECT * FROM UserDocuments WHERE CreatedBy = @CreatedBy";
                return await _dbConnection.QueryAsync<UserDocuments>(sql, new { CreatedBy = createdBy });
        }
        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertUserDocumentsAsync(UserDocuments documents)
        {
            documents.Id = Guid.NewGuid();
            string sql = $"INSERT INTO UserDocuments(Id,Created, CreatedBy, Updated, FilePath, FileType, ExpiryDate) VALUES (@Id,@Created, @CreatedBy, @Updated,@FilePath,@FileType , @ExpiryDate)";
            return await _dbConnection.ExecuteAsync(sql, new { Id = documents.Id, Created = documents.Created, CreatedBy = documents.CreatedBy, Updated = documents.Updated, documents.FilePath,documents.FileType, ExpiryDate = documents.ExpiryDate });
        }

        public async Task<int> UpdateUserDocumentsAsync(UserDocuments documents)
        {
            string sql = $"UPDATE UserDocuments SET Created = @Created, CreatedBy = @CreatedBy, Updated = @Updated, FilePath=@FilePath, FileType=@FileType, WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Created = documents.Created, CreatedBy = documents.CreatedBy, Updated = documents.Updated, FilePath=documents.FilePath, FileType=documents.FilePath, Id = documents.Id });
        }

        public async Task<int> DeleteUserDocumentsAsync(Guid id)
        {
            string sql = $"DELETE FROM UserDocuments WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        #endregion
    }
}
