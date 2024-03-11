using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;

namespace UWO_DailyCustodian.View;

public partial class SubmittedFormsPage : ContentPage
{
    public ObservableCollection<CustodianForm> Forms { get; } = new ObservableCollection<CustodianForm>(); 
    public SubmittedFormsPage()
	{
		InitializeComponent();

        Forms.Add(new CustodianForm { Building = "Building", CustodianName = "Custodian Name", Date = "Date" });
        Forms.Add(new CustodianForm { Building = "Building", CustodianName = "Custodian Name", Date = "Date" });
        Forms.Add(new CustodianForm { Building = "Building", CustodianName = "Custodian Name", Date = "Date" });

        this.BindingContext = this;
    }

	async void SubmitButtonClicked(object sender, EventArgs e)
	{
        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");
        await Navigation.PopToRootAsync();
    }
}