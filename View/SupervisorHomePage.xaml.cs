using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;

namespace UWO_DailyCustodian.View;

public partial class SupervisorHomePage : ContentPage
{
    public ObservableCollection<LeadForm> Forms { get; } = new ObservableCollection<LeadForm>();
    public SupervisorHomePage()
	{
		InitializeComponent();

		Forms.Add(new LeadForm { Building = "Building", LeadCustodianName = "Lead Custodian Name", Date = "Date" });
        Forms.Add(new LeadForm { Building = "Building", LeadCustodianName = "Lead Custodian Name", Date = "Date" });
        Forms.Add(new LeadForm { Building = "Building", LeadCustodianName = "Lead Custodian Name", Date = "Date" });

        this.BindingContext = this;
    }

    async void AddSupervisorClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddSupervisorPage());
    }

    async void ViewCodesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ViewCodesPage());
    }
}