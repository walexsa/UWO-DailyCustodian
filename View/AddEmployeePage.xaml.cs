using UWO_DailyCustodian.Model;
namespace UWO_DailyCustodian.View;

public partial class AddEmployeePage : ContentPage
{
    private IBusinessLogic businessLogic;
    private string role = "custodian";
    public AddEmployeePage()
	{
		InitializeComponent();
        businessLogic = new BusinessLogic();
    }

    async void CreateButtonClicked(object sender, EventArgs e)
    {
        if (emailENT.Text == null)
        {
            await DisplayAlert("Email not given", "Please provide an email for the employee you wish to add.", "OK");
        }
        bool success = await businessLogic.AddEmployee(emailENT.Text, role);
        await DisplayAlert("Success", "Employee added. Thank you!", "Go Back");
        await Navigation.PopAsync();
    }
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