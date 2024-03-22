using UWO_DailyCustodian.ViewModel;
namespace UWO_DailyCustodian.View;

public partial class LeadPasscodePage : ContentPage
{
	public LeadPasscodePage(LeadForm form)
	{
		InitializeComponent();

        BindingContext = MauiProgram.BusinessLogic;
        Form = form;
    }

    public LeadForm Form { get; set; }

    private async void NextButtonClicked(object sender, EventArgs e)
    {
        bool result = await MauiProgram.BusinessLogic.InsertLeadForm(Form);

        if (result == false)
        {
            await DisplayAlert("Oops!", "Form was not submitted, please try again.", "OK");
            return;
        }

        await Navigation.PushAsync(new SubmittedFormsPage());
    }
}