using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWO_DailyCustodian.ViewModel;

namespace UWO_DailyCustodian.Model
{
    public interface IDatabase
    {
        public Task<ObservableCollection<CustodianForm>> SelectAllCustodianForms();
        public Task<ObservableCollection<LeadForm>> SelectAllLeadForms();
        public Task<string> SignUp(string email, string password, string role);
        public Task<bool> SignIn(string email, string password);
        public Task<bool> InsertCustodianFormAsync(CustodianForm form);
        public Task<int> InsertLeadFormAsync(LeadForm form);
        public Task<bool> InsertFormRelation(int leadFormId, int custodianFormId);
        public Task<bool> InsertPhoto(byte[] fileData, string filePath);
    }
}
