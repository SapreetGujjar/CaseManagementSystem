using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class UserDocuments
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string FilePath { get; set; }
        public FileTypes FileType { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
    public enum FileTypes
    {
        ID = 1,
        Insurance = 2,
        ICO_Registration = 3,
        DBS = 4
    }
}
