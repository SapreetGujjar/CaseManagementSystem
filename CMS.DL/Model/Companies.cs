using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class Companies
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string CompanyName { get; set; }
        public string? KeyContact { get; set; }
        public string? KeyContactRole { get; set; }
        public string? Email { get; set; }
        public string? InvoiceEmail { get; set; }
        public byte CompanyType { get; set; }
        public string? Address { get; set; }
        public string AddressType { get; set; }
       // public string? OtherAddress { get; set; }
        public string CountryName { get; set; }
        public int CompanyId { get; set; }
        public List<CompaniesAddress> companyaddress { get; set; }
        public List<TextFieldModel> textFieldModels { get; set; }
        
        //public bool IsDefault { get; set; }   
    }



    public class TextFieldModel
    {
        public string Address { get; set; }
        public string AddressType { get; set; }
        public bool IsDefault { get; set; }
        public string value { get; set; }
        //public static TextFieldModel CreateNew()
        //{
        //    return new TextFieldModel();
        //}

        //public class AddressViewModel
        //{
        //    public Guid Id { get; set; }
        //    public string Address { get; set; }
        //    public string AddressType { get; set; }
        //    public bool IsDefault { get; set; }
        //}

    }
}

