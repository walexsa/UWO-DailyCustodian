using UWO_DailyCustodian.ViewModel;
namespace UWO_DailyCustodian.View;

public partial class CustodianPasscodePage : ContentPage
{
	public CustodianPasscodePage(CustodianForm form)
	{
		InitializeComponent();

        BindingContext = MauiProgram.BusinessLogic;
        Form = form;
    }

    public CustodianForm Form { get; set; }

    private async void SubmitButtonClicked(object sender, EventArgs e)
    {
        bool result = await MauiProgram.BusinessLogic.InsertCustodianForm(Form);

        if (result == false)
        {
            await DisplayAlert("Oops!", "Form was not submitted, please try again.", "OK");
            return;
        }

        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");
        await Navigation.PopAsync();
    }
}