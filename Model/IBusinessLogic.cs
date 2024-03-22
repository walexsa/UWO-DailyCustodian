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
        public Task<bool> InsertCustodianForm(CustodianForm form);
        public Task<bool> InsertLeadForm(LeadForm form);

    }
}
