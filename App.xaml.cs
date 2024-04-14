using UWO_DailyCustodian.View;

namespace UWO_DailyCustodian
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new View.TabbedPage();
        }
    }
}
