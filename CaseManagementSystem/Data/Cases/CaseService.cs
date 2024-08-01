using Azure.Core;
using CaseManagementSystem.Data.Subjects;
using CaseManagementSystem.Emails.Templates;
using CaseManagementSystem.Pages;
using CMS.DL.DM;
using CMS.DL.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace CaseManagementSystem.Data.Cases
{
    public class CaseService
    {
        private readonly string _connectionString = "";

        public CaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        #region GET
     




        //public async Task CreateCustomers(string accessToken)
        //{
        //    //var data = await GetCasesIsNotSynced();
        //    var apiUrl = "https://sandbox-quickbooks.api.intuit.com/v3/company/9341452129604540/customer";

        //    // Prepare the customer data
        //    var customerData = new
        //    {
        //        DisplayName = "Gautam4",
        //        GivenName = "Gautam4",
        //        FamilyName = "Gautam4",
        //        PrimaryEmailAddr = new
        //        {
        //            Address = "gautam4@example.com"
        //        },
        //        PrimaryPhone = new
        //        {
        //            FreeFormNumber = "(123) 456-7890"
        //        }
        //    };

        //    // Convert customer data to JSON
        //    var customerJson = JsonConvert.SerializeObject(customerData);
        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            // Set authorization header with the access token
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //            // Set content type header
        //            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            // Create the HTTP content with customer JSON data
        //            var httpContent = new StringContent(customerJson, Encoding.UTF8, "application/json");

        //            // Send the POST request to create the customer
        //            var response = await httpClient.PostAsync(apiUrl, httpContent);

        //            // Check if the request was successful
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseBody = await response.Content.ReadAsStringAsync();
        //                Console.WriteLine("Customer created successfully:");
        //                Console.WriteLine(responseBody);
        //            }
        //            else
        //            {
        //                // Handle error
        //                Console.WriteLine("Error creating customer:");
        //                Console.WriteLine(response.ReasonPhrase);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    // Create an HTTP request

        //}
        public async Task<IEnumerable<CaseViewModel>> GetAllCasesAsync()
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            IEnumerable<CMS.DL.Model.Cases> cases = await casesDM.GetAllCasesAsync();
            return cases.Select(c => new CaseViewModel
            {
                Id = c.Id,
                Created = c.Created,
                CreatedBy = c.CreatedBy,
                CreatedByName= c.CreatedByName,
                Updated = c.Updated,
                UpdatedBy = c.UpdatedBy,
                CompanyId = c.CompanyId,
                IndividualId = c.IndividualId,
                CaseNumber = c.CaseNumber,
                TraceLevelId = c.TraceLevelId,
                Fee = c.Fee,
                DebtAmount = c.DebtAmount,
                ClientRef = c.ClientRef,
                EndClient = c.EndClient,
                TraceReasonId = c.TraceReasonId,
                Notes = c.Notes,
                Status = c.Status,
                SubjectId = c.SubjectId,
                CaseLinkId = c.CaseLinkId,
                CompanyName = c.CompanyName,
                TraceLevelName = c.TraceLevelName,
                TraceReasonName = c.TraceReasonName,
                SubjectName = c.SubjectName,
                ClientName = c.ClientName,
                AgentName = c.AgentName,
                ClientReference = c.ClientReference,
                EndClientFreeText = c.EndClientFreeText,
                FilePath = c.FilePath
            });
        }
        public async Task<IEnumerable<CaseViewModel>> GetCasesIsNotSynced()
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            IEnumerable<CMS.DL.Model.Cases> cases = await casesDM.GetAllCasesAsync();
            cases = cases.Where(a => a.IsSynced == null || a.IsSynced == false);
            return cases.Select(c => new CaseViewModel
            {
                Id = c.Id,
                Created = c.Created,
                CreatedBy = c.CreatedBy,
                CreatedByName = c.CreatedByName,
                Updated = c.Updated,
                UpdatedBy = c.UpdatedBy,
                CompanyId = c.CompanyId,
                IndividualId = c.IndividualId,
                CaseNumber = c.CaseNumber,
                TraceLevelId = c.TraceLevelId,
                Fee = c.Fee,
                DebtAmount = c.DebtAmount,
                ClientRef = c.ClientRef,
                EndClient = c.EndClient,
                TraceReasonId = c.TraceReasonId,
                Notes = c.Notes,
                Status = c.Status,
                SubjectId = c.SubjectId,
                CaseLinkId = c.CaseLinkId,
                CompanyName = c.CompanyName,
                TraceLevelName = c.TraceLevelName,
                TraceReasonName = c.TraceReasonName,
                SubjectName = c.SubjectName,
                ClientName = c.ClientName,
                AgentName = c.AgentName,
                ClientReference = c.ClientReference,
                EndClientFreeText = c.EndClientFreeText,
                IsSynced = c.IsSynced,
                ClientAddress = c.ClientAddress,
                ClientTelephoneNo = c.ClientTelephoneNo,
                ClientEmail = c.ClientEmail


            });
        }
        public async Task<CaseViewModel> GetCasesByIdAsync(Guid id)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            CMS.DL.Model.Cases cases = await casesDM.GetCasesByIdAsync(id);

            if (cases != null)
            {
                return new CaseViewModel
                {
                    Id = cases.Id,
                    Created = cases.Created,
                    CreatedBy = cases.CreatedBy,
                    CreatedByName = cases.CreatedByName,
                    Updated = cases.Updated,
                    UpdatedBy = cases.UpdatedBy,
                    CompanyId = cases.CompanyId,
                    IndividualId = cases.IndividualId,
                    CaseNumber = cases.CaseNumber,
                    TraceLevelId = cases.TraceLevelId,
                    Fee = cases.Fee,
                    DebtAmount = cases.DebtAmount,
                    ClientRef = cases.ClientRef,
                    EndClient = cases.EndClient,
                    TraceReasonId = cases.TraceReasonId,
                    Notes = cases.Notes,
                    Status = cases.Status,
                    SubjectId = cases.SubjectId,
                    CaseLinkId = cases.CaseLinkId,
                    CompanyName = cases.CompanyName,
                    TraceLevelName = cases.TraceLevelName,
                    TraceReasonName = cases.TraceReasonName,
                    SubjectName = cases.SubjectName,
                    ClientName = cases.ClientName,
                    ClientEmail = cases.EmailAddress,
                    AgentName = cases.AgentName,
                    ClientReference = cases.ClientReference,
                    EndClientFreeText = cases.EndClientFreeText
                };
            }
            else
            {
                return null;
            }
        }
        public async Task<IEnumerable<CaseViewModel>> GetCasesByCreatedByAsync(Guid? id)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            IEnumerable<CMS.DL.Model.Cases> casesList = await casesDM.GetCasesByCreatedBy(id);

            if (casesList != null)
            {
                return casesList.Select(cases => new CaseViewModel
                {
                    Id = cases.Id,
                    Created = cases.Created,
                    CreatedBy = cases.CreatedBy,
                    CreatedByName = cases.CreatedByName,
                    Updated = cases.Updated,
                    UpdatedBy = cases.UpdatedBy,
                    CompanyId = cases.CompanyId,
                    IndividualId = cases.IndividualId,
                    CaseNumber = cases.CaseNumber,
                    TraceLevelId = cases.TraceLevelId,
                    Fee = cases.Fee,
                    DebtAmount = cases.DebtAmount,
                    ClientRef = cases.ClientRef,
                    EndClient = cases.EndClient,
                    TraceReasonId = cases.TraceReasonId,
                    Notes = cases.Notes,
                    Status = cases.Status,
                    SubjectId = cases.SubjectId,
                    CaseLinkId = cases.CaseLinkId,
                    CompanyName = cases.CompanyName,
                    TraceLevelName = cases.TraceLevelName,
                    TraceReasonName = cases.TraceReasonName,
                    SubjectName = cases.SubjectName,
                    ClientName = cases.ClientName,
                    ClientEmail = cases.EmailAddress,
                    AgentName = cases.AgentName,
                    ClientReference = cases.ClientReference,
                    EndClientFreeText = cases.EndClientFreeText
                });
            }
            else
            {
                return Enumerable.Empty<CaseViewModel>();
            }
        }

        public async Task<IEnumerable<CaseViewModel>> GetCasesByClientAsync(Guid? id)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            IEnumerable<CMS.DL.Model.Cases> cases = await casesDM.GetCasesByClientAsync(id);

            return cases.Select(c => new CaseViewModel
            {
                Id = c.Id,
                Created = c.Created,
                CreatedBy = c.CreatedBy,
                CreatedByName = c.CreatedByName,
                Updated = c.Updated,
                UpdatedBy = c.UpdatedBy,
                CompanyId = c.CompanyId,
                IndividualId = c.IndividualId,
                CaseNumber = c.CaseNumber,
                TraceLevelId = c.TraceLevelId,
                Fee = c.Fee,
                DebtAmount = c.DebtAmount,
                ClientRef = c.ClientRef,
                EndClient = c.EndClient,
                TraceReasonId = c.TraceReasonId,
                Notes = c.Notes,
                Status = c.Status,
                SubjectId = c.SubjectId,
                CaseLinkId = c.CaseLinkId,
                CompanyName = c.CompanyName,
                TraceLevelName = c.TraceLevelName,
                TraceReasonName = c.TraceReasonName,
                SubjectName = c.SubjectName,
                ClientName = c.ClientName,
                AgentName = c.AgentName,
                ClientReference = c.ClientReference,
                EndClientFreeText = c.EndClientFreeText,
                FilePath = c.FilePath
            });
        }

        public async Task<IEnumerable<CaseViewModel>> GetCasesByAgentAsync(Guid? id)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            IEnumerable<CMS.DL.Model.Cases> cases = await casesDM.GetCasesByAgentAsync(id);
            return cases.Select(c => new CaseViewModel
            {
                Id = c.Id,
                Created = c.Created,
                CreatedBy = c.CreatedBy,
                CreatedByName = c.CreatedByName,
                Updated = c.Updated,
                UpdatedBy = c.UpdatedBy,
                CompanyId = c.CompanyId,
                IndividualId = c.IndividualId,
                CaseNumber = c.CaseNumber,
                TraceLevelId = c.TraceLevelId,
                Fee = c.Fee,
                DebtAmount = c.DebtAmount,
                ClientRef = c.ClientRef,
                EndClient = c.EndClient,
                TraceReasonId = c.TraceReasonId,
                Notes = c.Notes,
                Status = c.Status,
                SubjectId = c.SubjectId,
                CaseLinkId = c.CaseLinkId,
                CompanyName = c.CompanyName,
                TraceLevelName = c.TraceLevelName,
                TraceReasonName = c.TraceReasonName,
                SubjectName = c.SubjectName,
                ClientName = c.ClientName,
                AgentName = c.AgentName,
                ClientReference = c.ClientReference,
                EndClientFreeText = c.EndClientFreeText
            });
        }
        public async Task<CaseViewModel> GetLastCaseAsync(string type)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            CMS.DL.Model.Cases cases = await casesDM.GetLastCaseAsync(type);

            if (cases != null)
            {
                return new CaseViewModel
                {
                    Id = cases.Id,
                    Created = cases.Created,
                    CreatedBy = cases.CreatedBy,
                    CreatedByName = cases.CreatedByName,
                    Updated = cases.Updated,
                    UpdatedBy = cases.UpdatedBy,
                    CompanyId = cases.CompanyId,
                    IndividualId = cases.IndividualId,
                    CaseNumber = cases.CaseNumber,
                    TraceLevelId = cases.TraceLevelId,
                    Fee = cases.Fee,
                    DebtAmount = cases.DebtAmount,
                    ClientRef = cases.ClientRef,
                    EndClient = cases.EndClient,
                    TraceReasonId = cases.TraceReasonId,
                    Notes = cases.Notes,
                    Status = cases.Status,
                    SubjectId = cases.SubjectId,
                    CaseLinkId = cases.CaseLinkId,
                    CompanyName = cases.CompanyName,
                    TraceLevelName = cases.TraceLevelName,
                    TraceReasonName = cases.TraceReasonName,
                    SubjectName = cases.SubjectName,
                    ClientName = cases.ClientName,
                    AgentName = cases.AgentName,
                    ClientReference = cases.ClientReference,
                    EndClientFreeText = cases.EndClientFreeText
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<Guid> GetCaseWatchApproved(Guid? Id)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            var id = await casesDM.GetCaseWatchApproved(Id);
            return id;
        }
        #endregion 
        public async Task<int> UpdateCaseTraceQuestions(Guid caseId, List<CaseTraceQuestions> traceQuestions)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            return await casesDM.UpdateCaseTraceQuestions(caseId, traceQuestions);
        }
        public async Task<List<int>> GetCaseNumbers(Guid? CompanyId, Guid? SubjectId)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            return await casesDM.GetCaseNumbers(CompanyId, SubjectId);
        }
        public async Task<Guid> GetCaseNumberId(int caseNumber)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            var result = await casesDM.GetCaseIdByCaseNumber(caseNumber);

            return result;
        }
        public async Task<IEnumerable<CaseViewModel>> GetDateFilter(DateTime startDate, DateTime endDate)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            var result = await casesDM.DateFilter(startDate, endDate);
            return result.Select(c => new CaseViewModel
            {
                Id = c.Id,
                Created = c.Created,
                CreatedBy = c.CreatedBy,
                CreatedByName = c.CreatedByName,
                Updated = c.Updated,
                UpdatedBy = c.UpdatedBy,
                CompanyId = c.CompanyId,
                IndividualId = c.IndividualId,
                CaseNumber = c.CaseNumber,
                TraceLevelId = c.TraceLevelId,
                Fee = c.Fee,
                DebtAmount = c.DebtAmount,
                ClientRef = c.ClientRef,
                EndClient = c.EndClient,
                TraceReasonId = c.TraceReasonId,
                Notes = c.Notes,
                Status = c.Status,
                SubjectId = c.SubjectId,
                CaseLinkId = c.CaseLinkId,
                CompanyName = c.CompanyName,
                TraceLevelName = c.TraceLevelName,
                TraceReasonName = c.TraceReasonName,
                SubjectName = c.SubjectName,
                ClientName = c.ClientName,
                AgentName = c.AgentName,
                ClientReference = c.ClientReference,
                EndClientFreeText = c.EndClientFreeText
            });
        }

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertCasesAsync(CaseViewModel caseViewModel)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            int result = await casesDM.InsertCasesAsync(new CMS.DL.Model.Cases
            {
                Created = caseViewModel.Created,
                CreatedBy = caseViewModel.CreatedBy,
                CreatedByName = caseViewModel.CreatedByName,
                Updated = caseViewModel.Updated,
                UpdatedBy = caseViewModel.UpdatedBy,
                CompanyId = (Guid)caseViewModel.CompanyId,
                IndividualId = caseViewModel.IndividualId,
                CaseNumber = caseViewModel.CaseNumber,
                TraceLevelId = caseViewModel.TraceLevelId,
                Fee = caseViewModel.Fee,
                DebtAmount = caseViewModel.DebtAmount,
                ClientRef = caseViewModel.ClientRef,
                EndClient = caseViewModel.EndClient,
                TraceReasonId = caseViewModel.TraceReasonId,
                Notes = caseViewModel.Notes,
                Status = (byte)caseViewModel.Status,
                SubjectId = caseViewModel.SubjectId,
                CaseLinkId = caseViewModel.CaseLinkId,
                ClientReference = caseViewModel.ClientReference,
                EndClientFreeText = caseViewModel.EndClientFreeText
            });

            return result;
        }

        public async Task<int> UpdateCasesAsync(CaseViewModel caseViewModel)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            int result = await casesDM.UpdateCasesAsync(new CMS.DL.Model.Cases
            {
                Id = caseViewModel.Id,
                Created = caseViewModel.Created,
                CreatedBy = caseViewModel.CreatedBy,
                CreatedByName = caseViewModel.CreatedByName,
                Updated = caseViewModel.Updated,
                UpdatedBy = caseViewModel.UpdatedBy,
                CompanyId = (Guid)caseViewModel.CompanyId,
                IndividualId = caseViewModel.IndividualId,
                CaseNumber = caseViewModel.CaseNumber,
                TraceLevelId = caseViewModel.TraceLevelId,
                Fee = caseViewModel.Fee,
                DebtAmount = caseViewModel.DebtAmount,
                ClientRef = caseViewModel.ClientRef,
                EndClient = caseViewModel.EndClient,
                TraceReasonId = caseViewModel.TraceReasonId,
                Notes = caseViewModel.Notes,
                Status = (byte)caseViewModel.Status,
                SubjectId = caseViewModel.SubjectId,
                CaseLinkId = caseViewModel.CaseLinkId,
                ClientReference = caseViewModel.ClientReference,
                EndClientFreeText = caseViewModel.EndClientFreeText,
                IsSynced = caseViewModel.IsSynced
            });
            return result;
        }

        public async Task<int> DeleteCasesAsync(Guid id)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            int result = await casesDM.DeleteCasesAsync(id);

            return result;
        }
        public async Task<int> GetApprovedCasesCount(Guid? id)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            int result = await casesDM.GetApprovedCasesCount(id);

            return result;
        }
        #endregion

        public async Task<int> SaveCaseReportFile(Guid CaseReportId, Guid? CaseId, Guid? SubjectId, string FilePath)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
           return await casesDM.SaveCaseReportFile(CaseReportId, CaseId, SubjectId, FilePath);

        }
        public async Task<Guid> DeleteCaseReportFile(Guid SubjectId)
        {
            CasesDM casesDM = new CasesDM(_connectionString);
            return await casesDM.DeleteCaseReportFile(SubjectId);
        }
        //public async Task<CaseViewModel> GetCaseReportFile(Guid? CaseId, Guid? SubjectId)
        //{
        //    try
        //    {
        //        CasesDM casesDM = new CasesDM(_connectionString);
        //        var Case = await casesDM.GetCaseReportFile(CaseId, SubjectId);
        //        return new CaseViewModel
        //        {
        //            Id = Case.Id,
        //            SubjectId = Case.SubjectId,
        //            FilePath = Case.FilePath,
        //            CaseReportId = Case.CaseReportId
        //        };
        //    }catch(Exception ex)
        //    {
        //        throw ex;
        //        return null;
        //    }
        //}

        public async Task<CaseViewModel> GetCaseReportFile(Guid? CaseId, Guid? SubjectId)
        {
            try
            {
                CasesDM casesDM = new CasesDM(_connectionString);
                var caseReport = await casesDM.GetCaseReportFile(CaseId, SubjectId);
                if (caseReport == null)
                {
                    return null;
                }
                return new CaseViewModel
                {
                    Id = caseReport.Id,
                    SubjectId = caseReport.SubjectId,
                    FilePath = caseReport.FilePath,
                    CaseReportId = caseReport.CaseReportId
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
