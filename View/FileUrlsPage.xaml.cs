namespace UWO_DailyCustodian.View;

public partial class FileUrlsPage : ContentPage
{
    public FileUrlsPage(List<string> urls)
    {
        InitializeComponent();

        // Loop through each URL and add a clickable label for each one
        foreach (var url in urls)
        {
            // Create a new Label for the URL
            Label linkLabel = new Label
            {
                Text = url,
                TextColor = Colors.Blue, // Set text color to blue to indicate a link
                TextDecorations = TextDecorations.Underline, // Underline the text
            };

            // Create a TapGestureRecognizer and add it to the Label
            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (sender, e) =>
            {
                // Open the URL in the default browser when the label is tapped
                await Launcher.OpenAsync(new Uri(url));
            };

            // Attach the gesture recognizer to the label
            linkLabel.GestureRecognizers.Add(tapGesture);

            // Add the Label to the StackLayout (linksStackLayout is defined in the XAML file)
            linksStackLayout.Children.Add(linkLabel);
        }
    }
}