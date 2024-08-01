using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMS.DL.Model
{
    public class Users
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? CompanyId { get; set; }
        public short? AgentType { get; set; }
        public string? AgentCompanyName { get; set; }
        public string? AgentCompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? AddressType { get; set; }
        public string? Address { get; set; }
        public string? TelphoneNumber { get; set; }
        public bool IsActive { get; set; }
        public byte? RoleType { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string NumberOfDays { get; set; }
        public string NewUpdated { get; set; }
        public string ccEmail = "admin@debtabase.com";
        public DateTime? ExpiringDate { get; set; }
        public List<UserTelephone> UserTelephones { get; set; } = new List<UserTelephone>();
    }
}
