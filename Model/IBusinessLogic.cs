using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWO_DailyCustodian.ViewModel;

namespace UWO_DailyCustodian.Model
{
    public interface IBusinessLogic
    {
        public Task<ObservableCollection<CustodianForm>> CustodianForms { get; }
        public Task<ObservableCollection<LeadForm>> LeadForms { get; }
        public Task<string> GetRole(string email);
        public Task<string> SignUp(string email, string password);
        public Task<bool> SignIn(string email, string password);
        public Task<bool> InsertCustodianForm(CustodianForm form);
        public Task<int> InsertLeadForm(LeadForm form);
        public Task<bool> InsertFormRelation(int leadFormId, int custodianFormId);
        public Task<bool> InsertPhoto(byte[] fileData, string filePath);
        public Task<bool> AddEditEmployee(string email, string role);
        public Task<bool> RemoveEmployee(string email);
        public Task DeleteLeadForms(List<LeadForm> forms);
        public Task<string> CreateAndUploadExcelDocument(LeadForm leadForm, int leadFormId, List<CustodianForm> custodianForms, string imagePath);
        public List<string> DownloadForms(List<LeadForm> forms);
    }
}
