using UWO_DailyCustodian.ViewModel;
namespace UWO_DailyCustodian.View;

public partial class CustodianFormPage : ContentPage
{
	public CustodianFormPage()
	{
		InitializeComponent();
	}

    private async void NextButtonClicked(object sender, EventArgs e)
    {
		CustodianForm form = new CustodianForm(firstName.Text, lastName.Text, building.Text, class_boards.IsChecked, class_garbage.IsChecked, class_floors.IsChecked, class_dusting.IsChecked, class_windows.IsChecked, class_walls.IsChecked, hall_floors.IsChecked, hall_garbage.IsChecked, hall_dusting.IsChecked, hall_walls.IsChecked, bath_sinks.IsChecked, bath_toilets.IsChecked, bath_dusting.IsChecked, bath_mirrors.IsChecked, bath_ledges.IsChecked, bath_dryers.IsChecked, bath_vents.IsChecked, bath_floors.IsChecked, bath_walls.IsChecked, bath_curtains.IsChecked, bath_shower.IsChecked, bath_supply.IsChecked, office_vacuum.IsChecked, stair_floors.IsChecked, stair_railings.IsChecked, stair_walls.IsChecked, entr_glass.IsChecked, entr_floors.IsChecked, entr_rugs.IsChecked, entr_dusting.IsChecked);
		await Navigation.PushAsync(new CustodianPasscodePage(form));
    }
}