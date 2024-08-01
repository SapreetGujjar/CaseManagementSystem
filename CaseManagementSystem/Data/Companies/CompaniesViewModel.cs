using CaseManagementSystem.Data.CompaniesAddress;
using CaseManagementSystem.Data.Enum;
using CMS.DL.Model;

namespace CaseManagementSystem.Data.Companies
{
    public class CompaniesViewModel
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CompanyName { get; set; } 
        public string? KeyContact { get; set; }
        public string? KeyContactRole { get; set; }
        public string? Email { get; set; }
        public string? InvoiceEmail { get; set; }
        public byte? CompanyType { get; set; } 
        public string CountryName { get; set; }
        public string? Address { get; set; }
        public string AddressType { get; set; }
      //  public string OtherAddress { get; set; }
        public Guid? companyId{ get; set; }
        public List<CompaniesAddressViewModel> CompanyAdd { get; set; } = new List<CompaniesAddressViewModel>();
        public IEnumerable<TextFieldModel> CompanyAddresses { get; set; } = new List<TextFieldModel>();
        //public bool IsDefault { get; set; }
        public string ccEmail = "admin@debtabase.com";
       
    }

    
}
