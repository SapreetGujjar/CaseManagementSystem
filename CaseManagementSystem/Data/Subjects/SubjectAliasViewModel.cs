using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagementSystem.Data.Subjects
{
    public class SubjectAliasViewModel
    {
        public Guid? SubjectAliasId { get; set; }
        public Guid SubjectId { get; set; }
        public string Alias { get; set; }
        public Guid? CaseId { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public bool Approved { get; set; } = false;
    }
}
