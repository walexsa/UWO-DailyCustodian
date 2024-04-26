using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWO_DailyCustodian.View;

public partial class SubmittedFormsPage : ContentPage
{
    public ObservableCollection<CustodianForm> Forms;
    private IBusinessLogic businessLogic;
    public LeadForm Form { get; set; }
    public byte[] ImageBytes { get; set; }
    public SubmittedFormsPage(LeadForm form, byte[] imageBytes)
	{
		InitializeComponent();

        this.BindingContext = this;
        Form = form;
        ImageBytes = imageBytes;

        InitializeFormsAsync();
    }

    private async Task InitializeFormsAsync()
    {
        businessLogic = new BusinessLogic();
        Forms = await businessLogic.CustodianForms;
        FormsCV.ItemsSource = Forms;
    }

    //private void FrameTapped(object sender, EventArgs e)
    //{
    //    var frame = sender as Frame;
    //    var selectedItem = frame?.BindingContext as CustodianForm;
    //    selectedItem.IsSelected = !selectedItem.IsSelected;
    //}

    async void SubmitButtonClicked(object sender, EventArgs e)
	{
        int leadFormId = await MauiProgram.BusinessLogic.InsertLeadForm(Form);

        if (leadFormId == -1)
        {
            await DisplayAlert("Oops!", "Form was not submitted, please try again.", "OK");
            return;
        }

        foreach (var selectedItem in FormsCV.SelectedItems)
        {
            if (selectedItem is CustodianForm form)
            {
                int custodianFormId = form.Id;
                bool success = await businessLogic.InsertFormRelation(leadFormId, custodianFormId);

                if (!success)
                {
                    await DisplayAlert("Something went wrong.", "Please try again.", "OK");
                    return;
                }
            }
        }

        if (ImageBytes != null)
        {
            string photoFilePath = leadFormId.ToString() + ".png";
            bool insertPhotoSuccess = await businessLogic.InsertPhoto(ImageBytes, photoFilePath);

            if (!insertPhotoSuccess)
            {
                await DisplayAlert("Image could not be saved.", "Please try again.", "OK");
                return;
            }
        }

        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");

        //LeadFormPage leadFormPage = Navigation.NavigationStack[0] as LeadFormPage;
        //if (leadFormPage != null)
        //{
        //    leadFormPage.shouldClearForm = true;
        //}
        Navigation.InsertPageBefore(new LeadFormPage(), Navigation.NavigationStack[0]);
        await Navigation.PopToRootAsync();
    }
}