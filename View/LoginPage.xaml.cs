using UWO_DailyCustodian.Model;
namespace UWO_DailyCustodian.View;

public partial class LoginPage : ContentPage
{
    private IBusinessLogic businessLogic;
    public LoginPage()
	{
		InitializeComponent();
        businessLogic = new BusinessLogic();
        emailENT.Text = Preferences.Get("UserEmail", "");
        passwordENT.Text = Preferences.Get("UserPassword", "");
    }

    async void OnLoginBtnClicked(object sender, EventArgs e)
    {
        bool success = await businessLogic.SignIn(emailENT.Text, passwordENT.Text);
        if (success)
        {
            Preferences.Set("UserEmail", emailENT.Text);
            Preferences.Set("UserPassword", passwordENT.Text);
            Application.Current.MainPage = new TabbedPage();
        } 
        else
        {
            await DisplayAlert("Login failed", "Please try again later.", "OK");
        }
    }

    async void FirstLoginTapped(object sender, TappedEventArgs args)
    {
        await Navigation.PushAsync(new FirstLoginPage());
    }
}