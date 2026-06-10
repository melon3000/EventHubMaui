using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly AuthService _authService;

    public ProfilePage()
    {
        InitializeComponent();
        _authService = ServiceHelper.GetService<AuthService>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Refresh();
    }

    private void Refresh()
    {
        if (_authService.CurrentUser is null)
        {
            NameLabel.Text = "Külaline";
            EmailLabel.Text = "Pole sisse logitud";
            RoleLabel.Text = "Roll: külaline";
            LoginButton.IsVisible = true;
            LogoutButton.IsVisible = false;
            return;
        }

        NameLabel.Text = _authService.CurrentUser.FullName;
        EmailLabel.Text = _authService.CurrentUser.Email;
        RoleLabel.Text = $"Roll: {_authService.CurrentUser.Role}";
        LoginButton.IsVisible = false;
        LogoutButton.IsVisible = true;
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WelcomePage());
    }

    private void LogoutButton_Clicked(object sender, EventArgs e)
    {
        _authService.Logout();
        Refresh();
    }
}
