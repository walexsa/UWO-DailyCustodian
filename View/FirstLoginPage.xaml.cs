using UWO_DailyCustodian.Model;
namespace UWO_DailyCustodian.View;

public partial class FirstLoginPage : ContentPage
{
    private IBusinessLogic businessLogic;
    public FirstLoginPage()
	{
		InitializeComponent();
        businessLogic = new BusinessLogic();
    }

    async void OnSignUpBtnClicked(object sender, EventArgs e)
    {
        if (!passwordENT.Text.Equals(password2ENT.Text))
        {
            await DisplayAlert("Passwords do not match", "Please try again.", "OK");
            return;
        }

        var response = await businessLogic.SignUp(emailENT.Text, passwordENT.Text);

        if (!response.Equals("success"))
        {
            await DisplayAlert("Something went wrong: " + response, "Please try again later.", "OK");
            return;
        }

        await DisplayAlert("Sign up successful", "Please check your email to confirm your account.", "OK");
        await Navigation.PopAsync();
    }
}