using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWO_DailyCustodian.ViewModel;

namespace UWO_DailyCustodian.Model
{
    public class BusinessLogic : IBusinessLogic
    {
        private IDatabase Database { get; set; }

        public Task<ObservableCollection<CustodianForm>> CustodianForms { get { return Database.SelectAllCustodianForms(); } }
        public Task<ObservableCollection<LeadForm>> LeadForms { get { return Database.SelectAllLeadForms(); } }

        public BusinessLogic()
        {
            Database = UWO_DailyCustodian.Model.Database.Instance;
        }

        public async Task<string> GetRole(string email)
        {
            return await Database.GetRole(email);
        }
        public async Task<string> SignUp(string email, string password)
        {
            return await Database.SignUp(email, password);
        }
        public async Task<bool> SignIn(string email, string password)
        {
            return await Database.SignIn(email, password);
        }

        public async Task<bool> InsertCustodianForm(CustodianForm form)
        {
            try
            {
                return await Database.InsertCustodianFormAsync(form);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
                return false;
            }
        }
        public async Task<int> InsertLeadForm(LeadForm form)
        {
            try
            {
                return await Database.InsertLeadFormAsync(form);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
                return -1;
            }
        }
        public async Task<bool> InsertFormRelation(int leadFormId, int custodianFormId)
        {
            try
            {
                return await Database.InsertFormRelation(leadFormId, custodianFormId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> InsertPhoto(byte[] fileData, string filePath)
        {
            try
            {
                return await Database.InsertPhoto(fileData, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddEditEmployee(string email, string role)
        {
            return await Database.AddEditEmployee(email, role);
        }

        public async Task<bool> RemoveEmployee(string email)
        {
            return await Database.RemoveEmployee(email);
        }

        public async Task DeleteLeadForms(List<LeadForm> forms)
        {
            await Database.DeleteLeadForms(forms);
        }

        public async Task<string> CreateAndUploadExcelDocument(LeadForm leadForm, int leadFormId, List<CustodianForm> custodianForms, string imagePath)
        {
            return await Database.CreateAndUploadExcelDocument(leadForm, leadFormId, custodianForms, imagePath);
        }
        public List<string> DownloadForms(List<LeadForm> forms)
        {
            return Database.DownloadForms(forms);
        }
    }
}
