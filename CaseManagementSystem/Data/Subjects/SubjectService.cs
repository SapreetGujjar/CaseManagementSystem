using CaseManagementSystem.Data.CaseTypes;
using CaseManagementSystem.Data.TraceLevels;
using CaseManagementSystem.Data.Users;
using CaseManagementSystem.Pages;
using CMS.DL.DM;
using CMS.DL.Model;

namespace CaseManagementSystem.Data.Subjects
{
    public class SubjectService
    {
        private readonly string _connectionString = "";

        public SubjectService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        #region GET

        public async Task<IEnumerable<SubjectViewModel>> GetAllSubjectsAsync()
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            IEnumerable<CMS.DL.Model.Subjects> subjects = await subjectDM.GetAllSubjectsAsync();

            return subjects.Select(c => c.ToVM());
        }

        public async Task<IEnumerable<SubjectViewModel>> GetSubjectsByClientAsync(Guid? id)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            IEnumerable<CMS.DL.Model.Subjects> subjects = await subjectDM.GetSubjectsByClientAsync(id);

            return subjects.Select(c => c.ToVM());
        }

        public async Task<IEnumerable<SubjectViewModel>> GetAllSubjectsWithCaseAsync()
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            IEnumerable<CMS.DL.Model.Subjects> subjects = await subjectDM.GetAllSubjectsWithCaseAsync();

            return subjects.Select(c => c.ToVM());
        }

        public async Task<SubjectViewModel> GetSubjectsByIdAsync(Guid id)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            CMS.DL.Model.Subjects subject = await subjectDM.GetSubjectsByIdAsync(id);

            return subject.ToVM();
        }

        public async Task<SubjectViewModel> GetLastSubjectsAsync()
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            CMS.DL.Model.Subjects subject = await subjectDM.GetLastSubjectsAsync();

            if (subject != null)
            {
                return subject.ToVM();
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<SubjectViewModel>> SearchSubjectsAsync(
            string firstName,
            string lastName,
            string alias,
            string postCode,
            string dob)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            IEnumerable<CMS.DL.Model.Subjects> subjects = await subjectDM.SearchSubjectsAsync(firstName, lastName,alias, postCode, dob);
            if (firstName != null || lastName != null || alias!= null || postCode != null || dob != null)
            {
                subjects = subjects.Where(s =>
              (string.IsNullOrEmpty(firstName) || s.FirstName.ToLower().Contains(firstName.ToLower()) ||
              (s.Alias != null && s.Alias.ToLower().Contains(firstName.ToLower()))) &&
              (string.IsNullOrEmpty(lastName) || s.LastName.ToLower().Contains(lastName.ToLower()) ||
             (s.Alias != null && s.Alias.ToLower().Contains(lastName.ToLower()))) &&
              (string.IsNullOrEmpty(postCode) || s.Addresses.ToLower().Contains(postCode.ToLower())) &&
              (string.IsNullOrEmpty(dob) || s.DateOfBirth?.ToString("yyyy-MM-dd") == dob)
          );
            }

            return subjects.Select(c => c.ToVM());
        }
            
        public async Task<IEnumerable<SubjectViewModel>> GetAllSubjectsWithCaseByAgentAsync(Guid id)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            IEnumerable<CMS.DL.Model.Subjects> subjects = await subjectDM.GetAllSubjectsWithCaseByAgentAsync(id);

            return subjects.Select(c => c.ToVM());
        }


        public async Task<IEnumerable<SubjectViewModel>> GetAllSubjectsWithCasetAsync(Guid id)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            IEnumerable<CMS.DL.Model.Subjects> subjects = await subjectDM.GetAllSubjectsWithCaseAsync(id);

            return subjects.Select(c => c.ToVM());
        }

        public async Task<SubjectViewModel> GetSubjectsWithCaseByCaseIdAsync(Guid id)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            CMS.DL.Model.Subjects subject = await subjectDM.GetSubjectsWithCaseByCaseIdAsync(id);

            if (subject != null)
            {
                return subject.ToVM();
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<SubjectChanges>> GetCaseChangesAsync(Guid caseId)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            return await subjectDM.GetChangesDoneOnACaseAsync(caseId);
        }

        public async Task<IEnumerable<SubjectCaseTraceQuestions>> GetTraceQuestionsAnswersAsync(Guid caseId)
        {
            SubjectsDM subjectDM = new SubjectsDM(_connectionString);
            return await subjectDM.GetTraceQuestionsAnswersAsync(caseId);
        }
        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<Guid> InsertSubjectsAsync(SubjectViewModel subjectViewModel)
        {
            SubjectsDM subjectsDM = new SubjectsDM(_connectionString);
            Guid result = await subjectsDM.InsertSubjectsAsync(subjectViewModel.ToModel());
            return result;
        }

        public async Task<int> UpdateSubjectsAsync(SubjectViewModel subjectViewModel)
        {
            SubjectsDM subjectsDM = new SubjectsDM(_connectionString);
            int result = await subjectsDM.UpdateSubjectsAsync(subjectViewModel.ToModel());
            return result;
        }

        public async Task<int> DeleteSubjectsAsync(Guid id)
        {
            SubjectsDM subjectsDM = new SubjectsDM(_connectionString);
            int result = await subjectsDM.DeleteSubjectsAsync(id);

            return result;
        }

        #endregion
    }
}
