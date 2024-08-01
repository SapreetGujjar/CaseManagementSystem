using CaseManagementSystem.Data.CompaniesAddress;
using CaseManagementSystem.Pages;
using CMS.DL.DM;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.Design;
using System.Net;

namespace CaseManagementSystem.Data.CompaniesAddress
{
    public class CompaniesAddressService
    {
        private readonly string _connectionString = "";

        public CompaniesAddressService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        #region GET

        public async Task<IEnumerable<CompaniesAddressViewModel>> GetAllCompaniesAddressAsync()
        {
            CompaniesAddressDM companiesAddressDM = new CompaniesAddressDM(_connectionString);
            IEnumerable<CMS.DL.Model.CompaniesAddress> companies = await companiesAddressDM.GetAllCompaniesAddressAsync();

            return companies.Select(c => c.ToVM());
        }

        public async Task<CompaniesAddressViewModel> GetCompaniesAddressByIdAsync(Guid id)
        {
            CompaniesAddressDM companiesAddressDM = new CompaniesAddressDM(_connectionString);
            CMS.DL.Model.CompaniesAddress company = await companiesAddressDM.GetCompaniesAddressByIdAsync(id);

            if (company != null)
            {
                return company.ToVM();
            }
            else
            {
                return null;
            }
        }

        public async Task<CompaniesAddressViewModel> GetLastCompaniesAddressAsync()
        {
            CompaniesAddressDM companiesAddressDM = new CompaniesAddressDM(_connectionString);
            CMS.DL.Model.CompaniesAddress company = await companiesAddressDM.GetLastCompaniesAddressAsync();

            if (company != null)
            {
                return company.ToVM();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertCompaniesAddressAsync(CompaniesAddressViewModel companiesView)
        {
            CompaniesAddressDM companiesAddressDM = new CompaniesAddressDM(_connectionString);
            CMS.DL.Model.CompaniesAddress companies = new CMS.DL.Model.CompaniesAddress
            {
                Id = companiesView.Id,
                Created = companiesView.Created,
                CreatedBy = companiesView.CreatedBy,
                Updated= companiesView.Updated,
                UpdatedBy = companiesView.UpdatedBy,
                Address = companiesView.Address,
                AddressType = companiesView.AddressType,
                IsDefault = companiesView.IsDefault,
                CompanyId = companiesView.CompanyId

            };

            int result = await companiesAddressDM.InsertCompaniesAddressAsync(companies);

            return result;
        }

        public async Task<int> UpadteCompaniesAddressAsync(CompaniesAddressViewModel companiesView)
        {
            CompaniesAddressDM companiesAddressDM = new CompaniesAddressDM(_connectionString);
            int result = await companiesAddressDM.UpdateCompaniesAddressAsync(companiesView.ToModel());

            return result;
        }

        public async Task<int> DeleteCompaniesAddressAsync(Guid id)
        {
            CompaniesAddressDM companiesAddressDM = new CompaniesAddressDM(_connectionString);
            int result = await companiesAddressDM.DeleteCompaniesAddressAsync(id);

            return result;
        }


    }
}
#endregion