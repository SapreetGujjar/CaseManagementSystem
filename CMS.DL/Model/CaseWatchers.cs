using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class CaseWatchers
    {
        public Guid CaseId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? Created { get; set; }
        public string FirstName { get; set; }
        public string WatcherEmailAddress { get; set; }
    }
}
