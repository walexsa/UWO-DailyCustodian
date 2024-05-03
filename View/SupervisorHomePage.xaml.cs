using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using Microsoft.Maui.Controls.Compatibility;

namespace UWO_DailyCustodian.View;

public partial class SupervisorHomePage : ContentPage
{
    public ObservableCollection<LeadForm> Forms;
    public ObservableCollection<LeadForm> FilteredForms { get; private set; }

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

    public SupervisorHomePage()
	{
		InitializeComponent();

        this.BindingContext = this;

        InitializeFormsAsync();
    }

    private async Task InitializeFormsAsync()
    {
        businessLogic = new BusinessLogic();
        Forms = await businessLogic.LeadForms;
        FilteredForms = new ObservableCollection<LeadForm>(Forms);
        FormsCV.ItemsSource = FilteredForms;
    }

    async void AddEmployeeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddEmployeePage());
    }

    private void FilterForms()
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            FilteredForms = new ObservableCollection<LeadForm>(Forms);
        }
        else
        {
            var filteredList = Forms.Where(form =>
                form.Building.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                form.LeadCustodianName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                form.Date.ToString("d").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            FilteredForms = new ObservableCollection<LeadForm>(filteredList);
        }

        FormsCV.ItemsSource = FilteredForms;
    }

    async void DeleteFormsClicked(object sender, EventArgs e)
    {
        bool deleteForms = await DisplayAlert("Deletion Confirmation", "Are you sure you want to delete all of these forms?", "Yes", "No, Go Back");
        if (deleteForms)
        {
            var selectedForms = FormsCV.SelectedItems.Cast<LeadForm>().ToList();

            List<int> formIds = new List<int>();
            foreach (var form in selectedForms)
            {
                formIds.Add(form.Id);
            }
            await businessLogic.DeleteLeadForms(formIds);

            foreach (var form in selectedForms)
            {
                Forms.Remove(form);
            }

            FilteredForms = new ObservableCollection<LeadForm>(Forms);
            FormsCV.ItemsSource = FilteredForms;
        }
    }

    async void DownloadFormsClicked(object sender, EventArgs e)
    {
        // Get the selected forms
        var selectedForms = FormsCV.SelectedItems.Cast<LeadForm>().ToList();

        // Implement your download logic here
        // For example, you might loop through the selected forms and save them to a file
        foreach (var form in selectedForms)
        {
            // Perform download action for each form
            //await DownloadFormAsync(form);
        }
    }
}