using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWO_DailyCustodian.View;

// Class for the SubmittedFormsPage, responsible for displaying submitted custodian forms to the leads
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
            OnPropertyChanged(nameof(SearchQuery)); // Notify that the SearchQuery property has changed
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
        FormsCV.ItemsSource = FilteredForms; // Set the item source of the forms collection view
    }

    // Method to filter forms based on the search query
    private void FilterForms()
    {
        if (string.IsNullOrWhiteSpace(searchQuery)) // Check if the search query is null or empty
        {
            FilteredForms = new ObservableCollection<CustodianForm>(Forms); // If empty, display all forms
        }
        else
        {
            // Filter the forms based on the search query
            var filteredList = Forms.Where(form =>
                form.Building.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                form.CustodianName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                form.Date.ToString("d").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            FilteredForms = new ObservableCollection<CustodianForm>(filteredList); // Update the filtered forms collection
        }

        FormsCV.ItemsSource = FilteredForms; // Set the item source of the forms collection view
    }

    async void SubmitButtonClicked(object sender, EventArgs e)
	{
        // Insert the lead form
        int leadFormId = await MauiProgram.BusinessLogic.InsertLeadForm(Form);

        if (leadFormId == -1)
        {
            await DisplayAlert("Oops!", "Form was not submitted, please try again.", "OK");
            return;
        }

        List<CustodianForm> custodianForms = new List<CustodianForm>();
        // Iterate through selected items in the forms collection view
        foreach (var selectedItem in FormsCV.SelectedItems)
        {
            if (selectedItem is CustodianForm form)
            {
                int custodianFormId = form.Id;
                custodianForms.Add(form); // Add the custodian form to the list
                bool success = await businessLogic.InsertFormRelation(leadFormId, custodianFormId); // Insert form relation

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
            photoFilePath = leadFormId.ToString() + ".png"; // Set photo file path
            bool insertPhotoSuccess = await businessLogic.InsertPhoto(ImageBytes, photoFilePath); // Insert photo

            if (!insertPhotoSuccess)
            {
                await DisplayAlert("Image could not be saved.", "Please try again.", "OK");
                return;
            }
        }

        // Create and upload Excel document
        await businessLogic.CreateAndUploadExcelDocument(Form, leadFormId, custodianForms, photoFilePath);

        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");

        // Navigate back to the LeadFormPage (a new one to clear the data)
        Navigation.InsertPageBefore(new LeadFormPage(), Navigation.NavigationStack[0]);
        await Navigation.PopToRootAsync();
    }
}