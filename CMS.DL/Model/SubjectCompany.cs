using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class SubjectCompany
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
