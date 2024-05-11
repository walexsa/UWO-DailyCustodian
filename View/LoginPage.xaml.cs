using Newtonsoft.Json;
using Supabase.Gotrue.Exceptions;
using UWO_DailyCustodian.Model;
namespace UWO_DailyCustodian.View;

// Class for the LoginPage, responsible for user login functionality
public partial class LoginPage : ContentPage
{
    private IBusinessLogic businessLogic;
    public LoginPage()
	{
		InitializeComponent();
        businessLogic = new BusinessLogic();

        // Prefill email and password fields with stored values
        emailENT.Text = Preferences.Get("UserEmail", "");
        passwordENT.Text = Preferences.Get("UserPassword", "");
    }

    async void OnLoginBtnClicked(object sender, EventArgs e)
    {
        try
        {
            // Attempt to sign in with provided email and password
            bool success = await businessLogic.SignIn(emailENT.Text, passwordENT.Text);
            if (success)
            {
                // Store email and password in preferences
                Preferences.Set("UserEmail", emailENT.Text);
                Preferences.Set("UserPassword", passwordENT.Text);

                // Get the role of the logged-in user and navigate to the appropriate page based on the user role
                string role = await businessLogic.GetRole(emailENT.Text);
                switch (role)
                {
                    case "custodian":
                        Application.Current.MainPage = new NavigationPage(new CustodianFormPage());
                        break;
                    case "lead":
                        Application.Current.MainPage = new NavigationPage(new LeadFormPage());
                        break;
                    case "supervisor":
                        Application.Current.MainPage = new NavigationPage(new SupervisorHomePage());
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

            // Display error description from the exception message
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
        // Navigate to the FirstLoginPage
        await Navigation.PushAsync(new FirstLoginPage());
    }
}