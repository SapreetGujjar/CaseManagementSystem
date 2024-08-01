using CMS.DL.Model;

namespace CaseManagementSystem.Data.Subjects
{
    public class SubjectViewModel
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Alias { get; set; }
        public string? Notes { get; set; }
        public string Email { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? Addresses { get; set; }
        public Guid? TitlePrefixId { get; set; }

        public List<SubjectAddressesViewModel> SubjectAddresses { get; set; } = new List<SubjectAddressesViewModel>();
        public List<SubjectAliasViewModel> SubjectAliases { get; set; } = new List<SubjectAliasViewModel>();
        public List<SubjectEmailViewModel> SubjectEmails { get; set; } = new List<SubjectEmailViewModel>();
        public List<SubjectCompanyViewModel> SubjectCompanies { get; set; } = new List<SubjectCompanyViewModel>();

        public List<SubjectTelephonesViewModel> SubjectTelephones { get; set; } = new List<SubjectTelephonesViewModel>();
        public string TitlePrfixName { get; set; }
        public string SubjectName { get; set; }
        public string ClientName { get; set; }
        public string AgentName { get; set; }
        public string CaseNumber { get; set; }
        public Guid? ClientRef { get; set; }
        public Guid? EndClient { get; set; }
        public string CompanyName { get; set; }
        public string Company { get; set; }

        public Guid? UserId { get; set; }
        public Guid? CaseId { get; set; }
        public DateTime? CaseCreated { get; set; }
        public string CaseNotes { get; set; }
        public byte? CaseStatus { get; set; } = 1;
    }
}
