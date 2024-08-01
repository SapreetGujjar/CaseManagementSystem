using CaseManagementSystem.Data.Documents;
using CMS.DL.DM;

namespace CaseManagementSystem.Data.UsersDocuments
{
    public class UserDocumentsService
    {
        private readonly string _connectionString = "";

        public UserDocumentsService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }
        #region GET

        public async Task<IEnumerable<UserDocumentsViewModel>> GetAllUserDocumentsAsync()
        {
            UserDocumentsDM documentsDM = new UserDocumentsDM(_connectionString);
            IEnumerable<CMS.DL.Model.UserDocuments> userDocuments = await documentsDM.GetAllUserDocumentsAsync();
            return userDocuments.Select(c => new UserDocumentsViewModel
            {
                Id = c.Id,
                Created = c.Created,
                CreatedBy = c.CreatedBy,
                Updated = c.Updated,
                FilePath = c.FilePath,
                FileType = (FileTypes)c.FileType
            });
        }
        public async Task<IEnumerable<UserDocumentsViewModel>> GetUserDocumentsByIdAsync(Guid createdBy)
        {
            UserDocumentsDM userDocumentsDM = new UserDocumentsDM(_connectionString);
            IEnumerable<CMS.DL.Model.UserDocuments> documents = await userDocumentsDM.GetUserDocumentsById(createdBy);

            if (documents != null)
            {
                return documents.Select(c=> new UserDocumentsViewModel
                //return new UserDocumentsViewModel
                {

                    Id = c.Id,
                    Created = c.Created,
                    CreatedBy = c.CreatedBy,
                    Updated = c.Updated,
                    FilePath=c.FilePath,
                    FileType = (FileTypes)c.FileType

                });
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertUserDocumentsAsync(UserDocumentsViewModel documentViewModel)
        {
            UserDocumentsDM documentsDM = new UserDocumentsDM(_connectionString);
            int result = await documentsDM.InsertUserDocumentsAsync(new CMS.DL.Model.UserDocuments
            {
                Id = new Guid(),
                Created = documentViewModel.Created,
                CreatedBy = documentViewModel.CreatedBy,
                Updated = documentViewModel.Updated,
                FilePath = documentViewModel.FilePath,
                FileType = (CMS.DL.Model.FileTypes)documentViewModel.FileType,
                ExpiryDate = documentViewModel.ExpiryDate
            });

            return result;
        }

        public async Task<int> UpdateUserDocumentsAsync(UserDocumentsViewModel documentViewModel)
        {
            UserDocumentsDM documentsDM = new UserDocumentsDM(_connectionString);
            int result = await documentsDM.UpdateUserDocumentsAsync(new CMS.DL.Model.UserDocuments
            {
                Id = documentViewModel.Id,
                Created = documentViewModel.Created,
                CreatedBy = documentViewModel.CreatedBy,
                Updated = documentViewModel.Updated,
                FilePath = documentViewModel.FilePath,
                FileType = (CMS.DL.Model.FileTypes)documentViewModel.FileType
            });

            return result;
        }

        public async Task<int> DeleteUserDocumentsAsync(Guid id)
        {
            UserDocumentsDM documentsDM = new UserDocumentsDM(_connectionString);
            int result = await documentsDM.DeleteUserDocumentsAsync(id);

            return result;
        }

        #endregion
    }
}
