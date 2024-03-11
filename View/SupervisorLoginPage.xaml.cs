namespace UWO_DailyCustodian.View;

public partial class SupervisorLoginPage : ContentPage
{
	public SupervisorLoginPage()
	{
		InitializeComponent();
	}

	async void OnLoginBtnClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SupervisorHomePage());
	}

    async void FirstLoginTapped(object sender, TappedEventArgs args)
    {
        await Navigation.PushAsync(new FirstLoginPage());
    }
}