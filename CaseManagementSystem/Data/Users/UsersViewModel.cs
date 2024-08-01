using CaseManagementSystem.Data.Enum;
using CMS.DL.Model;

namespace CaseManagementSystem.Data.Users
{
    public class UsersViewModel
    {
        public Guid Id { get; set; }
//        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? CompanyId { get; set; }
        public short? AgentType { get; set; }
        public string? AgentCompanyName { get; set; }

        public string? AgentCompanyAddress { get; set; }
       
        public string? CompanyName { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public byte? RoleType { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? AddressType { get; set; }
        public string? TelphoneNumber { get; set; }
    
        public bool IsDefault { get; set; }
        public string ccEmail = "admin@debtabase.com";
        public string GetActiveValue() {
            return (IsActive ? "Yes" : "No");
        }

        public string GetSiteAdminValue()
        {
            return "No";
            // return (IsSiteAdmin ? "Yes" : "No");
        }

        public string GetSuperUserValue()
        {
            return "No";
            // return (IsSuperUser ? "Yes" : "No");
        }

        public string GetRoleTypeName()
        {
            try
            {
                if (RoleType == null)
                    return string.Empty;

                RoleType roleType = (RoleType)RoleType;
                return roleType.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetFullName()
        {
            try
            {
                return FirstName + " " + LastName;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public List<string> AgentCompanyAddressList { get; set; }
        public List<string> AgentCompanyNameList { get; set; }
        public List<UserTelephoneViewModel> UserTelephones { get; set; } = new List<UserTelephoneViewModel>();
    }
}
