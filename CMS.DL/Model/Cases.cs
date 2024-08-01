using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class Cases
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? IndividualId { get; set; }
        public int CaseNumber { get; set; }
        public Guid? TraceLevelId { get; set; }
        public decimal? Fee { get; set; }
        public decimal? DebtAmount { get; set; }
        public Guid? ClientRef { get; set; }
        public Guid? EndClient { get; set; }
        public Guid? TraceReasonId { get; set; }
        public string? Notes { get; set; }
        public byte Status { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? CaseLinkId { get; set; }
        public string CompanyName { get; set; }
        public string TraceLevelName { get; set; }
        public string TraceReasonName { get; set; }
        public string SubjectName { get; set; }
        public string ClientName { get; set; }
        public string EmailAddress { get; set; }
        public string AgentName { get; set; }
        public string ClientReference { get; set; }
        public string EndClientFreeText { get; set; }
        public bool? IsSynced { get; set; }
        public string ClientAddress { get; set; }
        public string ClientTelephoneNo { get; set; }
        public string ClientEmail { get; set; }
        public Guid CaseReportId { get; set; }
        public string FilePath { get; set; }

    }
}
