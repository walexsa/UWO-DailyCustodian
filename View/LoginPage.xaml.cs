using Newtonsoft.Json;
using Supabase.Gotrue.Exceptions;
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
        try
        {
            bool success = await businessLogic.SignIn(emailENT.Text, passwordENT.Text);
            if (success)
            {
                Preferences.Set("UserEmail", emailENT.Text);
                Preferences.Set("UserPassword", passwordENT.Text);

                string role = await businessLogic.GetRole(emailENT.Text);
                switch (role)
                {
                    case "custodian":
                        Application.Current.MainPage = new CustodianFormPage();
                        break;
                    case "lead":
                        Application.Current.MainPage = new LeadFormPage();
                        break;
                    case "supervisor":
                        Application.Current.MainPage = new SupervisorHomePage();
                        break;
                    case "admin":
                        Application.Current.MainPage = new TabbedPage();
                        break;
                    default:
                        await DisplayAlert("Your account was not found in the UWO Custodian system.", "Please contact your supervisor if you think this is a mistake.", "OK");
                        break;
                }
            }
            else
            {
                await DisplayAlert("Login failed", "Please try again later.", "OK");
            }
        } catch (GotrueException ex)
        {
            var errorData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ex.Message);

            string errorDescription;
            if (errorData != null && errorData.TryGetValue("error_description", out errorDescription))
            {
                await DisplayAlert("Login failed", errorDescription, "OK");
            }
            else
            {
                await DisplayAlert("Login failed", ex.Message, "OK");
            }
        }
        
    }

    async void FirstLoginTapped(object sender, TappedEventArgs args)
    {
        await Navigation.PushAsync(new FirstLoginPage());
    }
}