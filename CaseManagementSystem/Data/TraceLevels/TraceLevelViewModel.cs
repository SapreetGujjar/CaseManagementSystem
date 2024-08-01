using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace CaseManagementSystem.Data.TraceLevels
{
    public class TraceLevelViewModel
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Name { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required] 
        public decimal AgentCost { get; set; }
        public string Description { get; set; }

        [RegularExpression("^[1-9]$", ErrorMessage = "Turnaround Time is Required")]
        [Required]
        public int TurnaroundTime { get; set; }

    }
}
