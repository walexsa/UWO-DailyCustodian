using UWO_DailyCustodian.ViewModel;
namespace UWO_DailyCustodian.View;

// Class for the CustodianFormPage, responsible for handling custodian form submission
public partial class CustodianFormPage : ContentPage
{
	public CustodianFormPage()
	{
		InitializeComponent();

        BindingContext = MauiProgram.BusinessLogic;
    }

    private async void NextButtonClicked(object sender, EventArgs e)
    {
        // Create a new CustodianForm object with the provided data
        CustodianForm form = new CustodianForm(firstName.Text, lastName.Text, building.Text, class_boards.IsChecked, class_garbage.IsChecked, class_floors.IsChecked, class_dusting.IsChecked, class_windows.IsChecked, class_walls.IsChecked, hall_floors.IsChecked, hall_garbage.IsChecked, hall_dusting.IsChecked, hall_walls.IsChecked, bath_sinks.IsChecked, bath_toilets.IsChecked, bath_dusting.IsChecked, bath_mirrors.IsChecked, bath_ledges.IsChecked, bath_dryers.IsChecked, bath_vents.IsChecked, bath_floors.IsChecked, bath_walls.IsChecked, bath_curtains.IsChecked, bath_shower.IsChecked, bath_supply.IsChecked, office_vacuum.IsChecked, stair_floors.IsChecked, stair_railings.IsChecked, stair_walls.IsChecked, entr_glass.IsChecked, entr_floors.IsChecked, entr_rugs.IsChecked, entr_dusting.IsChecked);
        // Insert the custodian form into the database
        bool result = await MauiProgram.BusinessLogic.InsertCustodianForm(form);

        if (result == false)
        {
            await DisplayAlert("Oops!", "Form was not submitted, please try again.", "OK");
            return;
        }

        // Clear the form fields after successful submission
        firstName.Text = string.Empty;
        lastName.Text = string.Empty;
        building.Text = string.Empty;

        class_boards.IsChecked = false;
        class_garbage.IsChecked = false;
        class_floors.IsChecked = false;
        class_dusting.IsChecked = false;
        class_windows.IsChecked = false;
        class_walls.IsChecked = false;

        hall_floors.IsChecked = false;
        hall_garbage.IsChecked = false;
        hall_dusting.IsChecked = false;
        hall_walls.IsChecked = false;

        bath_sinks.IsChecked = false;
        bath_toilets.IsChecked = false;
        bath_dusting.IsChecked = false;
        bath_mirrors.IsChecked = false;
        bath_ledges.IsChecked = false;
        bath_dryers.IsChecked = false;
        bath_vents.IsChecked = false;
        bath_floors.IsChecked = false;
        bath_walls.IsChecked = false;
        bath_curtains.IsChecked = false;
        bath_shower.IsChecked = false;
        bath_supply.IsChecked = false;

        office_vacuum.IsChecked = false;

        stair_floors.IsChecked = false;
        stair_railings.IsChecked = false;
        stair_walls.IsChecked = false;

        entr_glass.IsChecked = false;
        entr_floors.IsChecked = false;
        entr_rugs.IsChecked = false;
        entr_dusting.IsChecked = false;

        await DisplayAlert("Submission Confirmation", "Your form was successfully submitted. Thank you!", "OK");
    }
}