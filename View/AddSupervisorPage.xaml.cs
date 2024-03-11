namespace UWO_DailyCustodian.View;

public partial class AddSupervisorPage : ContentPage
{
	public AddSupervisorPage()
	{
		InitializeComponent();
	}

    async void CreateButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Success", "Username added. Thank you!", "Go Back");
        await Navigation.PopAsync();
    }
}