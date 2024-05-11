using UWO_DailyCustodian.Model;
namespace UWO_DailyCustodian.View;

// Class for the RemoveEmployeePage, responsible for removing an employee's account
public partial class RemoveEmployeePage : ContentPage
{
    private IBusinessLogic businessLogic;
    public RemoveEmployeePage()
	{
		InitializeComponent();
        businessLogic = new BusinessLogic();
    }

    async void RemoveButtonClicked(object sender, EventArgs e)
    {
        // Check if email is provided
        if (emailENT.Text == null)
        {
            await DisplayAlert("Email not given", "Please provide an email for the employee's account you wish to remove.", "OK");
            return;
        }

        // Ask for confirmation before removing the employee
        bool response = await DisplayAlert("Are you sure you want to remove " + emailENT.Text + " from the employee list?", "", "Yes", "No, Go Back");

        // If user confirms removal
        if (response)
        {
            // Attempt to remove the employee's account
            bool success = await businessLogic.RemoveEmployee(emailENT.Text);
            if (success) 
            {
                await DisplayAlert("Success", "Employee's account removed. Thank you!", "Go Back");
                await Navigation.PopAsync();
            } 
            else
            {
                await DisplayAlert("Something went wrong", "Check that the email provided is entered correctly and try again.", "OK");
                return;
            }
        }
        return;
    }
}