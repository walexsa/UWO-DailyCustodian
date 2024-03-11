namespace UWO_DailyCustodian.View;

public partial class LeadPasscodePage : ContentPage
{
	public LeadPasscodePage()
	{
		InitializeComponent();
	}

    private async void NextButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SubmittedFormsPage());
    }
}