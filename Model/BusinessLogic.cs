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
            Database = new Database();
        }

        public async Task<bool> InsertCustodianForm(CustodianForm form)
        {
            try
            {
                await Database.InsertCustodianFormAsync(form);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> InsertLeadForm(LeadForm form)
        {
            try
            {
                await Database.InsertLeadFormAsync(form);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
                return false;
            }
        }
    }
}
