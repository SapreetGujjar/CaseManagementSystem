using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class Subjects
    {
        #region Table Fields
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
        public string Company { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? Addresses { get; set; }
        public Guid? TitlePrefixId { get; set; }
        #endregion
        
        public List<SubjectAddresses> SubjectAddresses { get; set; } = new List<SubjectAddresses>();
        public List<SubjectAlias> SubjectAliases { get; set; } = new List<SubjectAlias>();
        public List<SubjectEmail> SubjectEmails { get; set; } = new List<SubjectEmail>();
        public List<SubjectCompany> SubjectCompanies { get; set; } = new List<SubjectCompany>();

        public List<SubjectTelephone> SubjectTelephones { get; set; } = new List<SubjectTelephone>();
        #region AddHoc fields
        public string TitlePrfixName { get; set; }
        public string SubjectName { get; set; }
        public string ClientName { get; set; }
        public string AgentName { get; set; }
        public string CaseNumber { get; set; }
        public Guid? ClientRef { get; set; }
        public Guid? EndClient { get; set; }
        public string CompanyName { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CaseId { get; set; }
        public DateTime? CaseCreated { get; set; }
        public string CaseNotes { get; set; }
        public byte? CaseStatus { get; set; } = 1;
        #endregion
    }
}
