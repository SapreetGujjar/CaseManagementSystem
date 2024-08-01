using CMS.DL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Metrics;

namespace CMS.DL.DM
{
    public class CompaniesDM
    {
        private readonly IDbConnection _dbConnection;

        public CompaniesDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        #region GET

        //public async Task<IEnumerable<Companies>> GetAllCompaniesAsync()
        //{
        //    string sql = "SELECT * FROM Companies";
        //    return await _dbConnection.QueryAsync<Companies>(sql);
        //}
        public async Task<IQueryable<Companies>> GetAllCompaniesAsync()
        {
            string sql = "SELECT * FROM Companies";
            var companies = await _dbConnection.QueryAsync<Companies>(sql);
            return companies.AsQueryable();
        }
        public async Task<IQueryable<Companies>> GetAllAgentCompaniesAsync()
        {
            string sql = "select * from Companies where CompanyType =1";
            var companies = await _dbConnection.QueryAsync<Companies>(sql);
            return companies.AsQueryable();
        }
        public async Task<IEnumerable<Companies>> GetSearchCompanies(string searchString)
        {
            string sql = "declare @SearchData varchar(max) = '';\r\n\r\nselect * from companies" +
                " \r\nwhere" +
                "\r\nCompanyName LIKE '%' + @SearchData + '%' or" +
                "\r\nKeyContact like '%' + @SearchData + '%'  or " +
                "\r\nKeyContactRole like '%' + @SearchData + '%' or " +
                "\r\n[Address] like '%' + @SearchData + '%';";
            return await _dbConnection.QueryAsync<Companies>(sql, new { SearchData  = searchString });
        }
        public async Task<Companies> GetCompaniesByIdAsync(Guid id)
        {
            try
            {
                string sql = $"SELECT * FROM Companies WHERE Id = @Id";
                var data = await _dbConnection.QueryFirstOrDefaultAsync<Companies>(sql, new { Id = id });


                if (data != null)
                {
                    string sql1 = $"SELECT * FROM CompaniesAddress WHERE CompanyId = @CompanyId";
                    data.companyaddress = (await _dbConnection.QueryAsync<CompaniesAddress>(sql1, new { CompanyId = data.Id })).ToList();

                }

                return data;
            }
            catch (Exception ex)
            {

                throw ex;
                
            }
            
        }

        public async Task<Companies> GetLastCompaniesAsync()
        {
            string sql = $"SELECT TOP 1 * FROM Companies ORDER BY Created DESC";
            return await _dbConnection.QueryFirstOrDefaultAsync<Companies>(sql);
        }



        //




        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<Guid> InsertCompaniesAsync(Companies company)
        {
            try
            {
                company.Id = Guid.NewGuid();
                string sql = $"INSERT INTO Companies(Id,Created, CreatedBy, Updated, UpdatedBy, CompanyName, KeyContact, KeyContactRole, Email, InvoiceEmail, CompanyType, Address, AddressType ,CountryName)" +
                    $" VALUES(@id,@Created, @CreatedBy, @Updated, @UpdatedBy, @CompanyName, @KeyContact, @KeyContactRole, @Email, @InvoiceEmail, @CompanyType, @Address, @AddressType, @CountryName)";
                var id = await _dbConnection.ExecuteAsync(sql, 
                  new {
                      id= company.Id,
                      Created = company.Created, 
                      CreatedBy = company.CreatedBy,
                      Updated = company.Updated, 
                      UpdatedBy = company.UpdatedBy,
                      CompanyName = company.CompanyName, 
                      KeyContact = company.KeyContact,
                      KeyContactRole = company.KeyContactRole, 
                      Email = company.Email, 
                      InvoiceEmail = company.InvoiceEmail, 
                      CompanyType = company.CompanyType, 
                      Address = company.Address,
                      AddressType = company.AddressType,
                   //   OtherAddress = company.OtherAddress,
                      CountryName = company.CountryName });
                return company.Id;
            }
            catch (Exception ex)
            {
                return Guid.NewGuid();
                throw ex;
            }
            
        }

        public async Task<int> UpdateCompaniesAsync(Companies company)
        {
            string sql = $"UPDATE Companies SET Created = @Created, CreatedBy = @CreatedBy,Updated = @Updated, UpdatedBy = @UpdatedBy, CompanyName = @CompanyName, KeyContact = @KeyContact, KeyContactRole = @KeyContactRole, Email = @Email, InvoiceEmail = @InvoiceEmail,CompanyType = @CompanyType, Address = @Address , AddressType =@AddressType , CountryName =@CountryName  WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Created = company.Created, CreatedBy = company.CreatedBy, Updated = company.Updated, UpdatedBy = company.UpdatedBy, CompanyName = company.CompanyName, KeyContact = company.KeyContact, KeyContactRole = company.KeyContactRole, Email = company.Email, InvoiceEmail = company.InvoiceEmail, CompanyType = company.CompanyType, company.Address, AddressType = company.AddressType,company.CountryName, Id = company.Id });
        }


        public async Task<int> DeleteCompaniesAsync(Guid id)
        {
            string sql = $"DELETE FROM CompaniesAddress WHERE CompanyId = @Id" 
                + " " + $"DELETE FROM Companies WHERE Id = @Id";
            

            return await _dbConnection.ExecuteAsync(sql, new { Id = id , CompanyId=id});
        }


        #endregion
    }
}
