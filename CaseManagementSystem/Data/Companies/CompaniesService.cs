using CaseManagementSystem.Data.Companies;
using CaseManagementSystem.Data.CompaniesAddress;
using CMS.DL.DM;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CaseManagementSystem.Data.Companies
{
    public class CompaniesService
    {
        private readonly string _connectionString = "";

        public CompaniesService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        #region GET

        //public async Task<IEnumerable<CompaniesViewModel>> GetAllCompaniesAsync()
        //{
        //    CompaniesDM companiesDM = new CompaniesDM(_connectionString);
        //    IEnumerable<CMS.DL.Model.Companies> companies = await companiesDM.GetAllCompaniesAsync();

        //    return companies.Select(c => c.ToVM());
        //}
        public async Task<IQueryable<CompaniesViewModel>> GetAllCompaniesAsync()
        {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            IQueryable<CMS.DL.Model.Companies> companies = await companiesDM.GetAllCompaniesAsync();

            return companies.Select(c => c.ToVM());
        }
         public async Task<IQueryable<CompaniesViewModel>> GetAllAgentCompaniesAsync()
        {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            IQueryable<CMS.DL.Model.Companies> companies = await companiesDM.GetAllAgentCompaniesAsync();

            return companies.Select(c => c.ToVM());
        }

        public async Task<IEnumerable<CompaniesViewModel>> GetSearchCompanies(string searchString)
        {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            IEnumerable<CMS.DL.Model.Companies> companies = await companiesDM.GetSearchCompanies(searchString);

            return companies.Select(c => c.ToVM());
        }

        public async Task<CompaniesViewModel> GetCompaniesByIdAsync(Guid id)
         {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            CMS.DL.Model.Companies company = await companiesDM.GetCompaniesByIdAsync(id);

            if (company != null)
            {
                var abd =  company.ToVM();
                //abd.CompanyAdd = CompaniesAddress.ToVM();

                for (int i = 0; i < company.companyaddress.Count; i++)
                {
                    abd.CompanyAdd.Add(new CompaniesAddressViewModel
                    {
                        Id = company.companyaddress[i].Id,
                        Address = company.companyaddress[i].Address.ToString(),
                        AddressType= company.companyaddress[i].AddressType,
                        IsDefault = company.companyaddress[i].IsDefault,

                    });;
                }
                return abd;
            }
            else
            {
                return null;
            }
        }

        public async Task<CompaniesViewModel> GetLastCompaniesAsync()
        {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            CMS.DL.Model.Companies company = await companiesDM.GetLastCompaniesAsync();

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

        public async Task<object> InsertCompaniesAsync(CompaniesViewModel companiesView)
        {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            object result = await companiesDM.InsertCompaniesAsync(companiesView.ToModel());

            return result;
        }

        public async Task<int> UpadteCompaniesAsync(CompaniesViewModel companiesView)
        {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            int result = await companiesDM.UpdateCompaniesAsync(companiesView.ToModel());

            return result;
        }

        public async Task<int> DeleteCompaniesAsync(Guid id)
        {
            CompaniesDM companiesDM = new CompaniesDM(_connectionString);
            int result = await companiesDM.DeleteCompaniesAsync(id);

            return result;
        }

        #endregion
    }
}
