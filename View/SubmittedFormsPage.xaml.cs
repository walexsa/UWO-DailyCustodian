using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWO_DailyCustodian.View;

public partial class SubmittedFormsPage : ContentPage
{
    public ObservableCollection<CustodianForm> Forms;
    public ObservableCollection<CustodianForm> FilteredForms { get; private set; }
    private IBusinessLogic businessLogic;
    private string searchQuery;
    public string SearchQuery
    {
        get => searchQuery;
        set
        {
            searchQuery = value;
            FilterForms();
            OnPropertyChanged(nameof(SearchQuery));
        }
    }
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
        FilteredForms = new ObservableCollection<CustodianForm>(Forms);
        FormsCV.ItemsSource = FilteredForms;
    }

    private void FilterForms()
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            FilteredForms = new ObservableCollection<CustodianForm>(Forms);
        }
        else
        {
            var filteredList = Forms.Where(form =>
                form.Building.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                form.CustodianName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                form.Date.ToString("d").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            FilteredForms = new ObservableCollection<CustodianForm>(filteredList);
        }

        FormsCV.ItemsSource = FilteredForms;
    }

    async void SubmitButtonClicked(object sender, EventArgs e)
	{
        int leadFormId = await MauiProgram.BusinessLogic.InsertLeadForm(Form);

        if (leadFormId == -1)
        {
            await DisplayAlert("Oops!", "Form was not submitted, please try again.", "OK");
            return;
        }

        List<CustodianForm> custodianForms = new List<CustodianForm>();
        foreach (var selectedItem in FormsCV.SelectedItems)
        {
            if (selectedItem is CustodianForm form)
            {
                int custodianFormId = form.Id;
                custodianForms.Add(form);
                bool success = await businessLogic.InsertFormRelation(leadFormId, custodianFormId);

                if (!success)
                {
                    await DisplayAlert("Something went wrong.", "Please try again.", "OK");
                    return;
                }
            }
        }

        string photoFilePath = null;
        if (ImageBytes != null)
        {
            photoFilePath = leadFormId.ToString() + ".png";
            bool insertPhotoSuccess = await businessLogic.InsertPhoto(ImageBytes, photoFilePath);

            if (!insertPhotoSuccess)
            {
                await DisplayAlert("Image could not be saved.", "Please try again.", "OK");
                return;
            }
        }

        await businessLogic.CreateAndUploadExcelDocument(Form, leadFormId, custodianForms, photoFilePath);

        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");

        Navigation.InsertPageBefore(new LeadFormPage(), Navigation.NavigationStack[0]);
        await Navigation.PopToRootAsync();
    }
}