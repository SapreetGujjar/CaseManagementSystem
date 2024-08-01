using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class Dashboard
    {
       public int NewCases { get; set; }
        public int ReviewCases { get; set; }
        public int ClosedCases { get; set; }

        public int Statustype { get; set; }
        public int StatusCount { get; set; }

        public List<int> Statustypee { get; set; }
        public List<int> StatusCountt { get; set; }
    }
}
