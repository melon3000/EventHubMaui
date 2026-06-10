using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class MyRegistrationsPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly EventService _eventService;
    private bool _isLoading;

    public MyRegistrationsPage()
    {
        InitializeComponent();
        _authService = ServiceHelper.GetService<AuthService>();
        _eventService = ServiceHelper.GetService<EventService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isLoading)
            return;

        _isLoading = true;

        try
        {
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Viga", $"Minu ürituste laadimine ebaõnnestus: {ex.Message}", "OK");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task LoadAsync()
    {
        if (!_authService.IsLoggedIn || _authService.CurrentUser is null)
        {
            RegistrationsCollection.ItemsSource = new List<object>();

            bool goLogin = await DisplayAlert(
                "Login required",
                "Selle vaate jaoks pead sisse logima.",
                "Login",
                "Cancel");

            if (goLogin)
                await Navigation.PushAsync(new WelcomePage());

            return;
        }

        // Admin on samuti kasutaja, seega tal võib olla 0 registreeringut.
        // See ei tohi rakendust kinni panna.
        var registrations = await _eventService.GetMyRegistrationsAsync(_authService.CurrentUser.UserId);
        RegistrationsCollection.ItemsSource = registrations;
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (_authService.CurrentUser is null)
                return;

            if (sender is Button button && button.CommandParameter is int eventId)
            {
                bool confirm = await DisplayAlert(
                    "Kinnitus",
                    "Kas soovid registreeringu tühistada?",
                    "Jah",
                    "Ei");

                if (!confirm)
                    return;

                await _eventService.CancelRegistrationAsync(_authService.CurrentUser.UserId, eventId);
                await LoadAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Viga", $"Tühistamine ebaõnnestus: {ex.Message}", "OK");
        }
    }
}
