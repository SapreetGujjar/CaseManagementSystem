using CMS.DL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.DM
{
    public class AddressTypeDM
    {
        private readonly IDbConnection _dbConnection;

        public AddressTypeDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        #region GET

        public async Task<IEnumerable<AddressType>> GetAllAddressTypeAsync()
        {
            string sql = "SELECT * FROM AddressType";
            return await _dbConnection.QueryAsync<AddressType>(sql);
        }

        public async Task<AddressType> GetAddressTypeByIdAsync(Guid id)
        {
            string sql = $"SELECT * FROM AddressType WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<AddressType>(sql, new { Id = id });
        }

        #endregion
        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertAddressTypeAsync(AddressType addressType)
        {
            try {
                addressType.Id = Guid.NewGuid();
                string sql = $"INSERT INTO AddressType(Id,Created, CreatedBy, Updated, UpdatedBy, Name) VALUES(@id,@Created, @CreatedBy, @Updated, @UpdatedBy, @Name)";
                return await _dbConnection.ExecuteAsync(sql, new {id=addressType.Id, Created = addressType.Created, CreatedBy = addressType.CreatedBy, Updated = addressType.Updated, UpdatedBy = addressType.UpdatedBy, Name = addressType.Name });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateAddressTypeAsync(AddressType addressType)
        {
            string sql = $"UPDATE AddressType SET Created = @Created, CreatedBy = @CreatedBy, Updated = @Updated, UpdatedBy = @UpdatedBy, Name = @Name WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Created = addressType.Created, CreatedBy = addressType.CreatedBy, Updated = addressType.Updated, UpdatedBy = addressType.UpdatedBy, Name = addressType.Name, Id = addressType.Id });
        }

        public async Task<int> DeleteAddressTypeAsync(Guid id)
        {
            string sql = $"DELETE FROM AddressType WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        #endregion
    }
}
