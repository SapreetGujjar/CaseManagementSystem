using CaseManagementSystem.Data.Subjects;
using CaseManagementSystem.Data.Users;
using Microsoft.AspNetCore.Components.Forms;
using CaseManagementSystem.Data.Users;

namespace CaseManagementSystem.Data.UsersDocuments
{
    public class UserDocumentsViewModel
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string FilePath { get; set; }
        public FileTypes FileType { get; set; }
        //  public byte[]? Data { get; set; }
        // public IReadOnlyList<IBrowserFile> files { get; set; }
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
