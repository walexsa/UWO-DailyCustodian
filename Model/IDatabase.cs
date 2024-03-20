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
        public Task<bool> InsertCustodianFormAsync(CustodianForm form);
    }
}
