using UWO_DailyCustodian.Model;
namespace UWO_DailyCustodian.View;

public partial class DeleteEmployeePage : ContentPage
{
    private IBusinessLogic businessLogic;
    public DeleteEmployeePage()
	{
		InitializeComponent();
        businessLogic = new BusinessLogic();
    }

    async void RemoveButtonClicked(object sender, EventArgs e)
    {
        if (emailENT.Text == null)
        {
            await DisplayAlert("Email not given", "Please provide an email for the employee you wish to remove.", "OK");
            return;
        }
        bool response = await DisplayAlert("Are you sure you want to remove " + emailENT.Text + " from the employee list?", "", "Yes", "No, Go Back");
        if (response)
        {
            bool success = await businessLogic.RemoveEmployee(emailENT.Text);
            if (success) 
            {
                await DisplayAlert("Success", "Employee added. Thank you!", "Go Back");
                await Navigation.PopAsync();
            } 
            else
            {
                await DisplayAlert("Something went wrong", "Please try again.", "OK");
                return;
            }
        }
        return;
    }
}