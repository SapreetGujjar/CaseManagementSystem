using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagementSystem.Data.Users
{
    public class UserTelephoneViewModel
    {
        public Guid? UserTelephoneId { get; set; } = new Guid();
        public string TelephoneNumber { get; set; }
        //public Guid? CreatedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
