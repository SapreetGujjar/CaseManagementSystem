using CaseManagementSystem.Data.Companies;
using CaseManagementSystem.Data.CompaniesAddress;
using CaseManagementSystem.Data.Subjects;
using CaseManagementSystem.Data.Users;
using CMS.DL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagementSystem.Data
{
    public static class Extensions
    {
        public static CMS.DL.Model.Users ToModel(this UsersViewModel usersView)
        {
            return new CMS.DL.Model.Users
            {
                Id = usersView.Id,
                UserName = usersView.EmailAddress, // usersView.UserName,
                Password = usersView.Password,
                FirstName = usersView.FirstName,
                LastName = usersView.LastName,
                CompanyId = usersView.CompanyId,
                AgentType = usersView.AgentType,
                AgentCompanyName = usersView.AgentCompanyName,
                AgentCompanyAddress = usersView.AgentCompanyAddress, 
                EmailAddress = usersView.EmailAddress,
                LastLogin = usersView.LastLogin,
                Address = usersView.Address,
                AddressType = usersView.AddressType,
                TelphoneNumber=usersView.TelphoneNumber,
                IsActive = usersView.IsActive,
                RoleType = usersView.RoleType,
                Created = usersView.Created,
                CreatedBy = usersView.CreatedBy,
                Updated = usersView.Updated,
                UpdatedBy = usersView.UpdatedBy,
                UserTelephones = usersView.UserTelephones.Select(telephone => new CMS.DL.Model.UserTelephone
                {
                    TelephoneNumber = telephone.TelephoneNumber,
                    Created = telephone.Created,
                    CreatedBy = telephone.CreatedBy,
                    Updated = telephone.Updated,
                    UpdatedBy = telephone.UpdatedBy
                }).ToList()
            };
        }

        public static UsersViewModel ToVM(this CMS.DL.Model.Users user)
        {
            return new UsersViewModel
            {
                Id = user.Id,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CompanyId = user.CompanyId,
                AgentType = user.AgentType,
                AgentCompanyName = user.AgentCompanyName,
                AgentCompanyAddress = user.AgentCompanyAddress,
                CompanyName = user.CompanyName,
                EmailAddress = user.EmailAddress,
                LastLogin = user.LastLogin,
                Address = user.Address,
                AddressType = user.AddressType,
                TelphoneNumber=user.TelphoneNumber,
                IsActive = user.IsActive,
                RoleType = user.RoleType,
                Created = user.Created,
                CreatedBy = user.CreatedBy,
                Updated = user.Updated,
                UpdatedBy = user.UpdatedBy,
                UserTelephones = user.UserTelephones.Select(telephone => new UserTelephoneViewModel
                {
                    TelephoneNumber = telephone.TelephoneNumber,
                    Created =  telephone.Created,
                    CreatedBy = telephone.CreatedBy,
                    Updated = telephone.Updated,
                    UpdatedBy = telephone.UpdatedBy
                }).ToList()
            };
        }

        public static CMS.DL.Model.Users ToVM(this UsersViewModel user)
        {
            return new CMS.DL.Model.Users
            {
                Id = user.Id,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CompanyId = user.CompanyId,
                AgentType = user.AgentType,
                AgentCompanyName = user.AgentCompanyName,
                AgentCompanyAddress = user.AgentCompanyAddress,
                CompanyName = user.CompanyName,
                EmailAddress = user.EmailAddress,
                LastLogin = user.LastLogin,
                Address = user.Address,
                TelphoneNumber = user.TelphoneNumber,
                IsActive = user.IsActive,
                RoleType = user.RoleType,
                Created = user.Created,
                CreatedBy = user.CreatedBy,
                Updated = user.Updated,
                UpdatedBy = user.UpdatedBy,
                UserTelephones = user.UserTelephones.Select(telephone => new UserTelephone
                {
                    TelephoneNumber = telephone.TelephoneNumber,
                    Created = telephone.Created,
                    CreatedBy = telephone.CreatedBy,
                    Updated = telephone.Updated,
                    UpdatedBy = telephone.UpdatedBy
                }).ToList()
            };
        }
        public static UserTelephone ToVM(this UserTelephoneViewModel userTelephone)
        {
            return new UserTelephone
            {
                TelephoneNumber = userTelephone.TelephoneNumber,           
                Created = userTelephone.Created,
                CreatedBy = userTelephone.CreatedBy,
                UserTelephoneId = userTelephone.UserTelephoneId
            };
        }
        public static List<UserTelephoneViewModel> ToVM(this List<CMS.DL.Model.UserTelephone> users)
        {
            return users.Select(user => new UserTelephoneViewModel
            {
                TelephoneNumber = user.TelephoneNumber,
                Created = user.Created,
                CreatedBy = user.CreatedBy,
                UserTelephoneId = user.UserTelephoneId
            }).ToList();
        }


        //public static CMS.DL.Model.UserTelephone ToModel(this UserTelephoneViewModel subjectTelephone)
        //{
        //    return new UserTelephone
        //    {
        //        TelephoneNumber = subjectTelephone.TelephoneNumber,
        //        Created = subjectTelephone.Created,
        //        CreatedBy = subjectTelephone.CreatedBy,
        //        UserTelephoneId = subjectTelephone.UserTelephoneId,

        //    };
        //}



        public static CMS.DL.Model.Subjects ToModel(this SubjectViewModel subjectViewModel)
        {
            return new CMS.DL.Model.Subjects
            {
                Id = subjectViewModel.Id,
                Created = subjectViewModel.Created,
                CreatedBy = subjectViewModel.CreatedBy,
                Updated = subjectViewModel.Updated,
                UpdatedBy = subjectViewModel.UpdatedBy,
                FirstName = subjectViewModel.FirstName,
                MiddleName = subjectViewModel.MiddleName,
                LastName = subjectViewModel.LastName,
                DateOfBirth = subjectViewModel.DateOfBirth,
                Alias = subjectViewModel.Alias,
                Notes = subjectViewModel.Notes,
                Email = subjectViewModel.Email,
                Company=subjectViewModel.Company,
                TelephoneNumber = subjectViewModel.TelephoneNumber,
                Addresses = subjectViewModel.Addresses,
                TitlePrefixId = subjectViewModel.TitlePrefixId,
                SubjectAddresses = subjectViewModel.SubjectAddresses.Select(sa => sa.ToModel()).ToList(),
                SubjectAliases = subjectViewModel.SubjectAliases.Select(sa => sa.ToModel()).ToList(),
                SubjectEmails = subjectViewModel.SubjectEmails.Select(sa => sa.ToModel()).ToList(),
                SubjectCompanies = subjectViewModel.SubjectCompanies.Select(sa => sa.ToModel()).ToList(),
                SubjectTelephones = subjectViewModel.SubjectTelephones.Select(st => st.ToModel()).ToList()
            };
        }
        public static SubjectViewModel ToVM(this CMS.DL.Model.Subjects subject) {
            return new SubjectViewModel
            {
                Id = subject.Id,
                Created = subject.Created,
                CreatedBy = subject.CreatedBy,
                Updated = subject.Updated,
                UpdatedBy = subject.UpdatedBy,
                FirstName = subject.FirstName,
                MiddleName = subject.MiddleName,
                LastName = subject.LastName,
                DateOfBirth = subject.DateOfBirth,
                Alias = subject.Alias,
                Notes = subject.Notes,
                Email = subject.Email,
                Company=subject.Company,
                TelephoneNumber = subject.TelephoneNumber,
                Addresses = subject.Addresses,
                TitlePrefixId = subject.TitlePrefixId,
                TitlePrfixName = subject.TitlePrfixName,
                SubjectName = subject.SubjectName,
                ClientName = subject.ClientName,
                AgentName = subject.AgentName,
                CaseNumber = subject.CaseNumber,
                ClientRef = subject.ClientRef,
                EndClient = subject.EndClient,
                CompanyName = subject.CompanyName,
                UserId = subject.UserId,
                CaseId = subject.CaseId,
                CaseStatus = subject.CaseStatus,
                SubjectAddresses = subject.SubjectAddresses.Select(sa => sa.ToVM()).ToList(),
                SubjectAliases = subject.SubjectAliases.Select(sa =>sa.ToVM()).ToList(),
                SubjectEmails = subject.SubjectEmails.Select(sa => sa.ToVM()).ToList(),
                SubjectCompanies = subject.SubjectCompanies.Select(sa => sa.ToVM()).ToList(),
                SubjectTelephones = subject.SubjectTelephones.Select(st => st.ToVM()).ToList()
            };
        }

        public static SubjectAddressesViewModel ToVM(this CMS.DL.Model.SubjectAddresses subjectAddress)
        {
            return new SubjectAddressesViewModel
            {
                Address = subjectAddress.Address,
                Approved = subjectAddress.Approved,
                CaseId = subjectAddress.CaseId,
                Created = subjectAddress.Created,
                CreatedBy = subjectAddress.CreatedBy,
                SubjectAddressId = subjectAddress.SubjectAddressId,
                SubjectId = subjectAddress.SubjectId
            };
        }

        public static CMS.DL.Model.SubjectAddresses ToModel(this SubjectAddressesViewModel subjectAddress)
        {
            return new CMS.DL.Model.SubjectAddresses
            {
                Address = subjectAddress.Address,
                Approved = subjectAddress.Approved,
                CaseId = subjectAddress.CaseId,
                Created = subjectAddress.Created,
                CreatedBy = subjectAddress.CreatedBy,
                SubjectAddressId = subjectAddress.SubjectAddressId,
                SubjectId = subjectAddress.SubjectId
            };
        }

        public static SubjectAliasViewModel ToVM(this CMS.DL.Model.SubjectAlias subjectAlias)
        {
            return new SubjectAliasViewModel
            {
                Alias = subjectAlias.Alias,
                Approved = subjectAlias.Approved,
                CaseId = subjectAlias.CaseId,
                Created = subjectAlias.Created,
                CreatedBy = subjectAlias.CreatedBy,
                SubjectAliasId = subjectAlias.SubjectAliasId,
                SubjectId = subjectAlias.SubjectId
            };
        }

        public static CMS.DL.Model.SubjectAlias ToModel(this SubjectAliasViewModel subjectAlias)
        {
            return new SubjectAlias
            {
                Alias = subjectAlias.Alias,
                Approved = subjectAlias.Approved,
                CaseId = subjectAlias.CaseId,
                Created = subjectAlias.Created,
                CreatedBy = subjectAlias.CreatedBy,
                SubjectAliasId = subjectAlias.SubjectAliasId,
                SubjectId = subjectAlias.SubjectId
            };
        }

        public static SubjectTelephonesViewModel ToVM(this CMS.DL.Model.SubjectTelephone subjectTelephone)
        {
            return new SubjectTelephonesViewModel
            {
                TelephoneNumber = subjectTelephone.TelephoneNumber,
                Approved = subjectTelephone.Approved,
                CaseId = subjectTelephone.CaseId,
                Created = subjectTelephone.Created,
                CreatedBy = subjectTelephone.CreatedBy,
                SubjectTelephoneId = subjectTelephone.SubjectTelephoneId,
                SubjectId = subjectTelephone.SubjectId
            };
        }

        public static CMS.DL.Model.SubjectTelephone ToModel(this SubjectTelephonesViewModel subjectTelephone)
        {
            return new SubjectTelephone
            {
                TelephoneNumber = subjectTelephone.TelephoneNumber,
                Approved = subjectTelephone.Approved,
                CaseId = subjectTelephone.CaseId,
                Created = subjectTelephone.Created,
                CreatedBy = subjectTelephone.CreatedBy,
                SubjectTelephoneId = subjectTelephone.SubjectTelephoneId,
                SubjectId = subjectTelephone.SubjectId
            };
        }

        public static SubjectEmailViewModel ToVM (this CMS.DL.Model.SubjectEmail subjectAlias)
        {
            return new SubjectEmailViewModel
            {
                Email = subjectAlias.Email,
                Approved = subjectAlias.Approved,
                CaseId = subjectAlias.CaseId,
                Created = subjectAlias.Created,
                CreatedBy = subjectAlias.CreatedBy,
                SubjectEmailId = subjectAlias.SubjectEmailId,
                SubjectId = subjectAlias.SubjectId
            };
        }

        public static CMS.DL.Model.SubjectEmail ToModel(this SubjectEmailViewModel subjectAlias)
        {
            return new CMS.DL.Model.SubjectEmail
            {
                Email = subjectAlias.Email,
                Approved = subjectAlias.Approved,
                CaseId = subjectAlias.CaseId,
                Created = subjectAlias.Created,
                CreatedBy = subjectAlias.CreatedBy,
                SubjectEmailId = subjectAlias.SubjectEmailId,
                SubjectId = subjectAlias.SubjectId
            };
        }

        public static SubjectCompanyViewModel ToVM(this CMS.DL.Model.SubjectCompany subjectAlias)
        {
            return new SubjectCompanyViewModel
            {
                Company = subjectAlias.Company,
                Approved = subjectAlias.Approved,
                CaseId = subjectAlias.CaseId,
                Created = subjectAlias.Created,
                CreatedBy = subjectAlias.CreatedBy,
                SubjectCompanyId = subjectAlias.SubjectCompanyId,
                SubjectId = subjectAlias.SubjectId
            };
        }

        public static CMS.DL.Model.SubjectCompany ToModel(this SubjectCompanyViewModel subjectAlias)
        {
            return new CMS.DL.Model.SubjectCompany
            {
                Company = subjectAlias.Company,
                Approved = subjectAlias.Approved,
                CaseId = subjectAlias.CaseId,
                Created = subjectAlias.Created,
                CreatedBy = subjectAlias.CreatedBy,
                SubjectCompanyId = subjectAlias.SubjectCompanyId,
                SubjectId = subjectAlias.SubjectId
            };
        }


        public static CompaniesViewModel ToVM(this CMS.DL.Model.Companies company)
        {
            
            var ad = new CompaniesViewModel
            {
                Id = company.Id,
                Created = company.Created,
                CreatedBy = company.CreatedBy,
                Updated = company.Updated,
                UpdatedBy = company.UpdatedBy,
                CompanyName = company.CompanyName,
                KeyContact = company.KeyContact,
                KeyContactRole = company.KeyContactRole,
                Email = company.Email,
                InvoiceEmail = company.InvoiceEmail,
                CompanyType = company.CompanyType,
                Address = company.Address,
                AddressType = company.AddressType,
                //OtherAddress = company.OtherAddress,
                CountryName = company.CountryName,
                
            };
            return ad;
        }

        

        public static CompaniesAddressViewModel ToVM(this CMS.DL.Model.CompaniesAddress company)
        {
            return new CompaniesAddressViewModel
            {
                Id = company.Id,
                Created = company.Created,
                CreatedBy = company.CreatedBy,
                Updated = company.Updated,
                UpdatedBy = company.UpdatedBy,
                Address = company.Address,
                AddressType = company.AddressType,
                IsDefault = company.IsDefault,


            };
        }

        public static CMS.DL.Model.Companies ToModel(this CompaniesViewModel companiesView)
        {
            return new CMS.DL.Model.Companies
            {
                Id = companiesView.Id,
                Created = companiesView.Created,
                CreatedBy = companiesView.CreatedBy,
                Updated = companiesView.Updated,
                UpdatedBy = companiesView.UpdatedBy,
                CompanyName = companiesView.CompanyName,
                KeyContact = companiesView.KeyContact,
                KeyContactRole = companiesView.KeyContactRole,
                Email = companiesView.Email,
                InvoiceEmail = companiesView.InvoiceEmail,
                CompanyType = Convert.ToByte(companiesView.CompanyType),
                Address = companiesView.Address,
                AddressType =  companiesView.AddressType,
              //  OtherAddress = companiesView.OtherAddress,
                CountryName = companiesView.CountryName,
            };
        }

        public static CMS.DL.Model.CompaniesAddress ToModel(this CompaniesAddressViewModel companiesView)
        {
            return new CMS.DL.Model.CompaniesAddress
            {
                Id = companiesView.Id,
                Created = companiesView.Created,
                CreatedBy = companiesView.CreatedBy,
                Updated = companiesView.Updated,
                UpdatedBy = companiesView.UpdatedBy,
                Address = companiesView.Address,
                AddressType = companiesView.AddressType,
                IsDefault=companiesView.IsDefault,
            };
        }

    }
}
