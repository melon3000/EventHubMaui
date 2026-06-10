using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class WelcomePage : ContentPage
{
    private readonly AuthService _authService;
    private bool _isSignUp;

    public WelcomePage()
    {
        InitializeComponent();
        _authService = ServiceHelper.GetService<AuthService>();
        SetMode(false);
    }

    private void SetMode(bool signUp)
    {
        _isSignUp = signUp;
        NameEntry.IsVisible = signUp;
        RememberBlock.IsVisible = !signUp;
        MainActionButton.Text = signUp ? "Sign Up!" : "Login";
        LoginTabButton.BackgroundColor = signUp ? Colors.Transparent : Colors.White;
        LoginTabButton.TextColor = signUp ? Color.FromArgb("#7C8495") : Color.FromArgb("#3B82F6");
        SignUpTabButton.BackgroundColor = signUp ? Colors.White : Colors.Transparent;
        SignUpTabButton.TextColor = signUp ? Color.FromArgb("#3B82F6") : Color.FromArgb("#7C8495");
    }

    private void LoginTabButton_Clicked(object sender, EventArgs e) => SetMode(false);

    private void SignUpTabButton_Clicked(object sender, EventArgs e) => SetMode(true);

    private async void MainActionButton_Clicked(object sender, EventArgs e)
    {
        if (_isSignUp)
        {
            var result = await _authService.RegisterAsync(NameEntry.Text ?? "", EmailEntry.Text ?? "", PasswordEntry.Text ?? "");
            await DisplayAlert(result.Success ? "Success" : "Error", result.Message, "OK");

            if (result.Success)
                await Navigation.PushAsync(new EventsPage());

            return;
        }

        var user = await _authService.LoginAsync(EmailEntry.Text ?? "", PasswordEntry.Text ?? "");
        if (user is null)
        {
            await DisplayAlert("Error", "Wrong email or password.", "OK");
            return;
        }

        await Navigation.PushAsync(new EventsPage());
    }

    private async void GuestButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventsPage());
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventsPage());
    }
}
