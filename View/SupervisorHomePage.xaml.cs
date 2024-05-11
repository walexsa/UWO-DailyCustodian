using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using Microsoft.Maui.Controls.Compatibility;
using System.Windows.Input;

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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitializeFormsAsync();
    }

    async void AddEmployeeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddEditEmployeePage());
    }

    async void RemoveEmployeeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RemoveEmployeePage());
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
        var selectedForms = FormsCV.SelectedItems.Cast<LeadForm>().ToList();
        int count = selectedForms.Count();
        bool deleteForms = await DisplayAlert("Deletion Confirmation", "Are you sure you want to delete " + count + " form(s)?", "Yes", "No, Go Back");
        if (deleteForms && count > 0)
        {
            await businessLogic.DeleteLeadForms(selectedForms);

            foreach (var form in selectedForms)
            {
                Forms.Remove(form);
                FormsCV.SelectedItems.Remove(form);
            }

            FilteredForms = new ObservableCollection<LeadForm>(Forms);
            FormsCV.ItemsSource = FilteredForms;
        }
    }

    async void DownloadFormsClicked(object sender, EventArgs e)
    {
        var selectedForms = FormsCV.SelectedItems.Cast<LeadForm>().ToList();

        List<string> urls = businessLogic.DownloadForms(selectedForms);

        await Navigation.PushAsync(new FileUrlsPage(urls));
    }
}