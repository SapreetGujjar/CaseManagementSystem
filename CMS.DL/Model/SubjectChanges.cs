using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class SubjectChanges
    {
        public string Change { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class SubjectCaseTraceQuestions : SubjectChanges
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
