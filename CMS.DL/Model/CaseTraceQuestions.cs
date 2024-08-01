using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class CaseTraceQuestions
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string CountryName { get; set; }
        public string Address { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
