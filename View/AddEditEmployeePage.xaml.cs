using UWO_DailyCustodian.Model;
namespace UWO_DailyCustodian.View;

// Class for the Add/Edit Employee page, responsible for managing employee creation or update
public partial class AddEditEmployeePage : ContentPage
{
    private IBusinessLogic businessLogic;
    private string role = "custodian";
    public AddEditEmployeePage()
	{
		InitializeComponent();
        businessLogic = new BusinessLogic();
    }

    async void CreateButtonClicked(object sender, EventArgs e)
    {
        if (emailENT.Text == null)
        {
            await DisplayAlert("Email not given", "Please provide an email for the employee you wish to add or update.", "OK");
        }
        bool success = await businessLogic.AddEditEmployee(emailENT.Text, role);
        if (success)
        {
            await DisplayAlert("Success", "Employee list has been updated. Thank you!", "Go Back");
            await Navigation.PopAsync();
        } else
        {
            await DisplayAlert("Something went wrong.", "Please try again later.", "OK");
        }
    }

    // Update the role when a different button is clicked
    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton)
        {
            if (e.Value)
            {
                string selectedValue = radioButton.Value as string;
                role = selectedValue;
            }
        }
    }
}