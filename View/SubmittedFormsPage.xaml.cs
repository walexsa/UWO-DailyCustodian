using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;
using UWO_DailyCustodian.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWO_DailyCustodian.View;

public partial class SubmittedFormsPage : ContentPage
{
    private ObservableCollection<CustodianForm> _forms;

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<CustodianForm> Forms
    {
        get { return _forms; }
        set
        {
            _forms = value;
            OnPropertyChanged();
        }
    }
    public SubmittedFormsPage()
	{
		InitializeComponent();

        IBusinessLogic businessLogic = new BusinessLogic();

        Forms = new ObservableCollection<CustodianForm>();
        InitializeFormsAsync();

        this.BindingContext = this;
    }

    private async Task InitializeFormsAsync()
    {
        IBusinessLogic businessLogic = new BusinessLogic();
        Forms = await businessLogic.CustodianForms;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    async void SubmitButtonClicked(object sender, EventArgs e)
	{
        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");
        await Navigation.PopToRootAsync();
    }
}