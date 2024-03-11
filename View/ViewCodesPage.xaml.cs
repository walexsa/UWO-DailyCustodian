namespace UWO_DailyCustodian.View;

public partial class ViewCodesPage : ContentPage
{
	public ViewCodesPage()
	{
		InitializeComponent();

		DateTime today = DateTime.Today;
		todayLabel.Text = today.ToString("D");
		DateTime tomorrow = today.AddDays(1);
		tomorrowLabel.Text = tomorrow.ToString("D");
		DateTime today2 = today.AddDays(2);
		today2Label.Text = today2.ToString("D");
        DateTime today3 = today.AddDays(3);
		today3Label.Text = today3.ToString("D");
        DateTime today4 = today.AddDays(4);
		today4Label.Text = today4.ToString("D");
        DateTime today5 = today.AddDays(5);
		today5Label.Text = today5.ToString("D");
        DateTime today6 = today.AddDays(6);
		today6Label.Text = today6.ToString("D");
    }
}