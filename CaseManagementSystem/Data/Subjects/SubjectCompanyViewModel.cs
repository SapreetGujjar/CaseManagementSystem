namespace CaseManagementSystem.Data.Subjects
{
    public class SubjectCompanyViewModel
    {
        public Guid? SubjectCompanyId { get; set; }
        public Guid SubjectId { get; set; }
        public string Company { get; set; }
        public Guid? CaseId { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool Approved { get; set; } = false;
    }
}
