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
        await Navigation.PushAsync(new SubmittedFormsPage(Form));
    }
}