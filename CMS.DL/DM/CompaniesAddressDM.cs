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
    public class CompaniesAddressDM 
    {
        private readonly IDbConnection _dbConnection;
       

        public CompaniesAddressDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }
        #region GET
        public async Task<IEnumerable<CompaniesAddress>> GetAllCompaniesAddressAsync()
        {
            string sql = "SELECT * FROM CompaniesAddress";
            return await _dbConnection.QueryAsync<CompaniesAddress>(sql);
        }

        public async Task<CompaniesAddress> GetCompaniesAddressByIdAsync(Guid id)
        {
            string sql = $"SELECT * FROM CompaniesAddress WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<CompaniesAddress>(sql, new { Id = id });
        }

        public async Task<CompaniesAddress> GetLastCompaniesAddressAsync()
        {
            string sql = $"SELECT TOP 1 * FROM CompaniesAddress ORDER BY Created DESC";
            return await _dbConnection.QueryFirstOrDefaultAsync<CompaniesAddress>(sql);
        }
        #endregion


        #region INSERT/UPDATE/DELETE
        public async Task<int> InsertCompaniesAddressAsync(CompaniesAddress company)
        {
         
            try {
                string sql =
              $"INSERT INTO CompaniesAddress(Id, Created, CreatedBy, Updated, UpdatedBy, Address, AddressType,IsDefault,CompanyId) " +
              $"VALUES(NEWID(), @Created, @CreatedBy, @Updated, @UpdatedBy, @Address, @AddressType,@IsDefault, @CompanyId)";
             

                //string sql =$"INSERT INTO CompaniesAddress(Id, Created, CreatedBy, Updated, UpdatedBy, Address, AddressType,IsDefault,CompanyId) " +
                //    $"VALUES(NEWID(), @Created, @CreatedBy, @Updated, @UpdatedBy, @Address, @AddressType,@IsDefault, @CompanyId)";

                return await _dbConnection.ExecuteAsync(sql, new { Created = company.Created, CreatedBy = company.CreatedBy, Updated = company.Updated, UpdatedBy = company.UpdatedBy, Address = company.Address, AddressType = company.AddressType, IsDefault = company.IsDefault, CompanyId = company.CompanyId });
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            }
        public async Task<int> UpdateCompaniesAddressAsync(CompaniesAddress company)
        {
            string sql = $"UPDATE CompaniesAddress SET Created = @Created, CreatedBy = @CreatedBy,Updated = @Updated, UpdatedBy = @UpdatedBy, Address = @Address , AddressType =@AddressType,IsDefault=@IsDefault WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Created = company.Created, CreatedBy = company.CreatedBy, Updated = company.Updated, UpdatedBy = company.UpdatedBy, company.Address, company.AddressType, company.IsDefault, Id = company.Id });
        }
        public async Task<int> DeleteCompaniesAddressAsync(Guid id)
        {
            string sql = $"DELETE FROM CompaniesAddress WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        #endregion
    }
}
