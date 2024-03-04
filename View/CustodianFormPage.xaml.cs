namespace UWO_DailyCustodian.View;

public partial class CustodianFormPage : ContentPage
{
	public CustodianFormPage()
	{
		InitializeComponent();
	}

    private async void NextButtonClicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new CustodianPasscodePage());
    }
}