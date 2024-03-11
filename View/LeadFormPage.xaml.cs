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
        await Navigation.PushAsync(new LeadPasscodePage());
    }
}