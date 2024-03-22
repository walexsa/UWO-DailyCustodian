using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWO_DailyCustodian.View;

public partial class SubmittedFormsPage : ContentPage
{
    private ObservableCollection<CustodianForm> _forms;

    public ObservableCollection<CustodianForm> Forms;
    private IBusinessLogic businessLogic;
    public SubmittedFormsPage()
	{
		InitializeComponent();

        this.BindingContext = this;

        InitializeFormsAsync();
    }

    private async Task InitializeFormsAsync()
    {
        businessLogic = new BusinessLogic();
        Forms = await businessLogic.CustodianForms;
        FormsCV.ItemsSource = Forms;
    }

    async void SubmitButtonClicked(object sender, EventArgs e)
	{
        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");
        await Navigation.PopToRootAsync();
    }
}