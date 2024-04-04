using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using Microsoft.Maui.Controls.Compatibility;

namespace UWO_DailyCustodian.View;

public partial class SupervisorHomePage : ContentPage
{
    public ObservableCollection<LeadForm> Forms;
    private IBusinessLogic businessLogic;
    public SupervisorHomePage()
	{
		InitializeComponent();

        this.BindingContext = this;

        InitializeFormsAsync();
    }

    private async Task InitializeFormsAsync()
    {
        businessLogic = new BusinessLogic();
        Forms = await businessLogic.LeadForms;
        FormsCV.ItemsSource = Forms;
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