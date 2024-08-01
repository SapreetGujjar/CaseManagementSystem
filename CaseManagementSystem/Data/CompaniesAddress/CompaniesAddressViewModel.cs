

namespace CaseManagementSystem.Data.CompaniesAddress

{
    public class CompaniesAddressViewModel
    {
        public Guid Id { get; set; } 
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? Address { get; set; }
        public string? AddressType { get; set; }
        public bool IsDefault { get; set; }
        public string OtherAddress { get; set; }

        public Guid CompanyId { get; set; }

    }
}
