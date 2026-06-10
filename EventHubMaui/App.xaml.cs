using EventHubMaui.Pages;

namespace EventHubMaui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new WelcomePage())
        {
            BarBackgroundColor = Color.FromArgb("#3B82F6"),
            BarTextColor = Colors.White
        };
    }
}
