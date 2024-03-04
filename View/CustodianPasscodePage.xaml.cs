namespace UWO_DailyCustodian.View;

public partial class CustodianPasscodePage : ContentPage
{
	public CustodianPasscodePage()
	{
		InitializeComponent();
	}

    private async void SubmitButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");
        await Navigation.PopAsync();
    }
}