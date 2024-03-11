namespace UWO_DailyCustodian.View;

public partial class FirstLoginPage : ContentPage
{
	public FirstLoginPage()
	{
		InitializeComponent();
	}

    async void OnLoginBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SupervisorHomePage());
    }
}