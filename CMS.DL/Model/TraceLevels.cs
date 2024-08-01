using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class TraceLevels
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Name { get; set; }
        public decimal Fee { get; set; }
        public decimal AgentCost { get; set; }
        public string Description { get; set; }
        public int TurnaroundTime { get; set; }
    }
}
