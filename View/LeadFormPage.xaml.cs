using UWO_DailyCustodian.ViewModel;
namespace UWO_DailyCustodian.View;

public partial class LeadFormPage : ContentPage
{
    public LeadFormPage()
	{
		InitializeComponent();
	}

    void SelectPhotosClicked(object sender, EventArgs e)
    {
        
    }

    async void NextButtonClicked(object sender, EventArgs e)
    {
        LeadForm form = new LeadForm(firstName.Text, lastName.Text, building.Text, 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
        await Navigation.PushAsync(new LeadPasscodePage(form));
    }
}