using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMS.DL.Model
{
    public class CompaniesAddress
    {
        public Guid Id { get; set; } = new Guid();
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? Address { get; set; }
        //public Companies companies { get; set; }
        public string? AddressType { get; set; }
        public bool IsDefault { get; set; }
        public List<CompaniesAddress> companiesAddress { get; set; } = new List<CompaniesAddress>();

        public Guid CompanyId { get; set; } = new Guid();

    }
}
