using System.Windows.Markup;
using UWO_DailyCustodian.ViewModel;
namespace UWO_DailyCustodian.View;

public partial class LeadFormPage : ContentPage
{
    public LeadFormPage()
	{
		InitializeComponent();
	}

    private int ClassBoards { get; set; }
    private int ClassGarbage { get; set; }
    private int ClassFloors { get; set; }
    private int ClassDusting { get; set; }
    private int ClassWindows { get; set; }
    private int ClassWalls { get; set; }
    private int HallFloors { get; set; }
    private int HallGarbage { get; set; }
    private int HallDusting { get; set; }
    private int HallWalls { get; set; }
    private int BathSinks { get; set; }
    private int BathToilets { get; set; }
    private int BathDusting { get; set; }
    private int BathMirrors { get; set; }
    private int BathLedges { get; set; }
    private int BathDryers { get; set; }
    private int BathVents { get; set; }
    private int BathFloors { get; set; }
    private int BathWalls { get; set; }
    private int BathCurtains { get; set; }
    private int BathShower { get; set; }
    private int BathSupply { get; set; }
    private int OfficeVacuum { get; set; }
    private int StairFloors { get; set; }
    private int StairRailings { get; set; }
    private int StairWalls { get; set; }
    private int EntrGlass { get; set; }
    private int EntrFloors { get; set; }
    private int EntrRugs { get; set; }
    private int EntrDusting { get; set; }

    void OnCheckedChanged(object sender, EventArgs e)
    {
        RadioButton rb = sender as RadioButton;

        if (rb != null && rb.IsChecked == true)
        {
            int value = int.Parse(rb.Value.ToString());
            switch (rb.GroupName)
            {
                case "class_boards": ClassBoards = value; break;
                case "class_garbage": ClassGarbage = value; break;
                case "class_floors": ClassFloors = value; break;
                case "class_dusting": ClassDusting = value; break;
                case "class_windows": ClassWindows = value; break;
                case "class_walls": ClassWalls = value; break;
                case "hall_floors": HallFloors = value; break;
                case "hall_garbage": HallGarbage = value; break;
                case "hall_dusting": HallDusting = value; break;
                case "hall_walls": HallWalls = value; break;
                case "bath_sinks": BathSinks = value; break;
                case "bath_toilets": BathToilets = value; break;
                case "bath_dusting": BathDusting = value; break;
                case "bath_mirrors": BathMirrors = value; break;
                case "bath_ledges": BathLedges = value; break;
                case "bath_dryers": BathDryers = value; break;
                case "bath_vents": BathVents = value; break;
                case "bath_floors": BathFloors = value; break;
                case "bath_walls": BathWalls = value; break;
                case "bath_curtains": BathCurtains = value; break;
                case "bath_shower": BathShower = value; break;
                case "bath_supply": BathSupply = value; break;
                case "office_vacuum": OfficeVacuum = value; break;
                case "stair_floors": StairFloors = value; break;
                case "stair_railings": StairRailings = value; break;
                case "stair_walls": StairWalls = value; break;
                case "entr_glass": EntrGlass = value; break;
                case "entr_floors": EntrFloors = value; break;
                case "entr_rugs": EntrRugs = value; break;
                case "entr_dusting": EntrDusting = value; break;
                default: break;
            }
        }
    }

    async void SelectPhotosClicked(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                // save photo in db
            }
        }
    }

    async void NextButtonClicked(object sender, EventArgs e)
    {
        LeadForm form = new LeadForm(firstName.Text, lastName.Text, building.Text, ClassBoards, ClassGarbage, ClassFloors, ClassDusting, ClassWindows, ClassWalls, HallFloors, HallGarbage, HallDusting, HallWalls, BathSinks, BathToilets, BathDusting, BathMirrors, BathLedges, BathDryers, BathVents, BathFloors, BathWalls, BathCurtains, BathShower, BathSupply, OfficeVacuum, StairFloors, StairRailings, StairWalls, EntrGlass, EntrFloors, EntrRugs, EntrDusting, remarks.Text);
        await Navigation.PushAsync(new LeadPasscodePage(form));
    }
}